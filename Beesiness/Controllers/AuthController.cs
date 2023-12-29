using Beesiness.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Beesiness.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        private const int PageSize = 6;
        [Authorize(Roles = "Root")]
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RequestRegistrationIndex(string searchString, string filterType, string sortOrder, int pageNumber = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["CurrentFilter"] = searchString;

                var usuarios = from u in _context.tblUsuariosTemporales
                               select u;
                
                if (!String.IsNullOrEmpty(searchString))
                {
                    switch (filterType)
                    {
                        case "Nombre":
                            usuarios = usuarios.Where(u => u.Nombre.Contains(searchString));
                            break;
                        case "Correo":
                            usuarios = usuarios.Where(u => u.Correo.Contains(searchString));
                            break;
                        case "Rol":
                            usuarios = usuarios.Where(u => u.Rol.Contains(searchString));
                            break;
                    }
                }
                switch (sortOrder)
                {
                    case "fecha_asc":
                        usuarios = usuarios.OrderBy(u => u.FechaSolicitud);
                        break;
                    case "fecha_desc":
                        usuarios = usuarios.OrderByDescending(u => u.FechaSolicitud);
                        break;
                        // ... otros casos de ordenamiento ...
                }
                /*switch (sortOrder) DESHABILITADO
                {
                    case "id_desc":
                        usuarios = usuarios.OrderByDescending(u => u.Id);
                        break;
                    case "nombre":
                        usuarios = usuarios.OrderBy(u => u.Nombre);
                        break;
                    case "nombre_desc":
                        usuarios = usuarios.OrderByDescending(u => u.Nombre);
                        break;
                    case "correo":
                        usuarios = usuarios.OrderBy(u => u.Correo);
                        break;
                    case "correo_desc":
                        usuarios = usuarios.OrderByDescending(u => u.Correo);
                        break;
                    case "rol":
                        usuarios = usuarios.OrderBy(u => u.Rol);
                        break;
                    case "rol_desc":
                        usuarios = usuarios.OrderByDescending(u => u.Rol);
                        break;
                    case "fecha":
                        usuarios = usuarios.OrderBy(u => u.FechaSolicitud);
                        break;
                    case "fecha_desc":
                        usuarios = usuarios.OrderByDescending(u => u.FechaSolicitud);
                        break;
                    default:
                        usuarios = usuarios.OrderBy(u => u.Id);
                        break;
                }*/

                int totalUsers = await usuarios.CountAsync();
                int totalPages = (int)Math.Ceiling(totalUsers / (double)PageSize);

                var pagedUsers = await usuarios
                        .Skip((pageNumber - 1) * PageSize)
                        .Take(PageSize)
                        .ToListAsync();
                var model = new RequestRegistrationIndexViewModel
                {
                    Usuarios = pagedUsers,
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling(totalUsers / (double)PageSize)
                };

                if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_UserListPartial", model);
                }
                return View(model);
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePendiente(int id)
        {
            var usuario = await _context.tblUsuariosTemporales.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.tblUsuariosTemporales.Remove(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(RequestRegistrationIndex));
        }

        public IActionResult ContraseñaOlvidada()
        {
            return View();
        }
        public IActionResult RequestRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RequestRegistration(LoginRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(p => p.ErrorMessage)).ToList();
                var existingTempUser = await _context.tblUsuariosTemporales.FirstOrDefaultAsync(u => u.Correo == model.Registration.Correo);
                var existingUser = await _context.tblUsuarios.FirstOrDefaultAsync(u => u.Correo == model.Registration.Correo);

                if (existingTempUser != null || existingUser != null)
                {
                    ModelState.AddModelError("Registration.Correo", "Este correo ya está registrado.");
                    return View("LoginIn", model);
                }

                var usuarioTemporal = new UsuarioTemporal
                {
                    Nombre = model.Registration.NombreCompleto,
                    Correo = model.Registration.Correo,
                    Rol = model.Registration.RolSeleccionado
                };

                _context.tblUsuariosTemporales.Add(usuarioTemporal);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Tu solicitud ha sido enviada exitosamente. Recibirás un correo cuando tu solicitud haya sido aprobada.";
                return RedirectToAction("LoginIn", "Auth");
            }

            return View("LoginIn", model); // Si no es válido o hay un error, regresamos el modelo a la vista.
        }


        [HttpGet]
        public IActionResult LoginIn(Usuario U)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginIn(LoginRegistrationViewModel Lvm)
        {
            var usuarios = _context.tblUsuarios.ToList();
            if (usuarios.Count == 0)
            {
                //primero qye todo revisaremos si existen roles creados
                //en la practica esto sera innecesario si es que la app ya esta asociada a una BD nuestra
                var roles = _context.tblRoles.ToList();
                if (roles.Count == 0) //tambien habria que poner una condicion por si hay roles pero no hay Root
                {
                    //creamos primero un rol para asignar al superUsuario
                    Rol rol = new Rol();
                    //rol.Id = 1; // tendremos que confiar que nadie "jugara" con la base de datos de antemano
                    rol.Nombre = "Root";
                    rol.Descripcion = "Super Usuario administrador de los otros usuarios y del funcionamiento del sistema";
                    _context.tblRoles.Add(rol);
                    _context.SaveChanges();
                }

                //Ahora creamos el usuario que sera Root o Super Usuario
                Usuario U = new Usuario();
                U.Correo = "yonathanherreracl@gmail.com";
                U.Nombre = "Yonathan Herrera";
                U.IdRol = 1;

                CreatePasswordHash("leica666", out byte[] passwordHash, out byte[] passwordSalt);

                U.PasswordHash = passwordHash;
                U.PasswordSalt = passwordSalt;
                _context.tblUsuarios.Add(U);
                _context.SaveChanges();
            }


            var us = _context.tblUsuarios.Include(u => u.Rol) // Asegurémonos de incluir el rol asociado
                            .Where(u => u.Correo.Equals(Lvm.Login.Correo)).FirstOrDefault();

            if (us != null)
            {
                //Usuario Encontrado
                if (VerificarPass(Lvm.Login.Password, us.PasswordHash, us.PasswordSalt))
                {
                    //Usuario y contraseña correctos!
                    var Claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, Lvm.Login.Correo),
            new Claim(ClaimTypes.Name, us.Nombre),
            new Claim(ClaimTypes.Role, us.Rol.Nombre)  // Añade el nombre del rol como un Claim
        };

                    var identity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                        new AuthenticationProperties { IsPersistent = true });

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //Contraseña incorrecta
                    ModelState.AddModelError("", "Contraseña incorrecta");
                    return View(Lvm);
                }
            }
            else
            {
                //Usuario no existe
                ModelState.AddModelError("", "Usuario no encontrado");
                return View(Lvm);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LoginIn", "Auth"); // Redirige al usuario a la página principal o de inicio de sesión.
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //administrador
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerificarPass(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var pass = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return pass.SequenceEqual(passwordHash);
            }
        }
    }
}
