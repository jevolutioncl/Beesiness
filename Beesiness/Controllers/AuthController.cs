using Beesiness.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult RequestRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RequestRegistration(RegistrationRequestViewModel model)
        {
            // Mapear el ViewModel a la entidad UsuarioTemporal
            var usuarioTemporal = new UsuarioTemporal
            {
                Nombre = model.NombreCompleto,
                Correo = model.Correo,
                Rol = model.RolSeleccionado
            };

            // Guardar en la base de datos
            _context.tblUsuariosTemporales.Add(usuarioTemporal);
            await _context.SaveChangesAsync();

            // Redirigir a una página de confirmación o donde quieras después de registrar al usuario
            return RedirectToAction("Confirmacion");
        }

        [HttpGet]
        public IActionResult LoginIn(Usuario U)
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> LoginIn(LoginViewModel Lvm)
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


            var us = _context.tblUsuarios.Where(u => u.Correo.Equals(Lvm.Correo)).FirstOrDefault();
            if (us != null)
            {
                //Usuario Encontrado
                if (VerificarPass(Lvm.Password, us.PasswordHash, us.PasswordSalt))
                {
                    //Usuario y contraseña correctos!
                    var Claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, Lvm.Correo),
                        new Claim(ClaimTypes.Name, us.Nombre)
                    };

                    var identity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                        new AuthenticationProperties { IsPersistent = true});

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

            return View();    
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
