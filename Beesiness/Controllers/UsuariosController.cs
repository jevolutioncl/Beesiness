using Beesiness.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Beesiness.Controllers
{
    [Authorize(Roles = "Root")]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        private const int PageSize = 6; // Para configurar los atributos de la página
        public async Task<IActionResult> GestionUsuario(string searchString, string filterType, int pageNumber = 1)
        {
            ViewData["CurrentFilter"] = searchString;

            var usuarios = from u in _context.tblUsuarios.Include(u => u.Rol) select u;

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
                        usuarios = usuarios.Where(u => u.Rol.Nombre.Contains(searchString));
                        break;
                }
            }

            /* Ordenar por nombre (deshabilitado)
            switch (sortOrder)
            {
                case "nombre_desc":
                    usuarios = usuarios.OrderByDescending(u => u.Nombre);
                    break;
                default:
                    usuarios = usuarios.OrderBy(u => u.Nombre);
                    break;
            } */

            int totalUsers = await usuarios.CountAsync();
            int totalPages = (int)Math.Ceiling(totalUsers / (double)PageSize);

            var pagedUsers = await usuarios
                    .Skip((pageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();
            // Crear modelo con la nueva paginación
            var model = new EnfermedadViewModel
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


        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var usuario = _context.tblUsuarios.Include(u => u.Rol).FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                IdRolSeleccionado = usuario.IdRol,
                RolesDisponibles = _context.tblRoles.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Nombre
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            ModelState.Remove("RolesDisponibles");
            // Remover validaciones de contraseña si no se desea cambiar
            if (string.IsNullOrWhiteSpace(model.NewPassword) && string.IsNullOrWhiteSpace(model.ConfirmNewPassword))
            {
                ModelState.Remove("NewPassword");
                ModelState.Remove("ConfirmNewPassword");
            }

            if (ModelState.IsValid)
            {
                var usuario = await _context.tblUsuarios.FindAsync(model.Id);
                if (usuario == null)
                {
                    return NotFound();
                }

                // Actualizar campos básicos
                usuario.Nombre = model.Nombre;
                usuario.Correo = model.Correo;
                usuario.IdRol = model.IdRolSeleccionado;

                if (!string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(model.NewPassword, out passwordHash, out passwordSalt);

                    usuario.PasswordHash = passwordHash;
                    usuario.PasswordSalt = passwordSalt;
                }

                // Guardar los cambios en la base de datos
                _context.Update(usuario);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(GestionUsuario));
            }

            model.RolesDisponibles = _context.tblRoles.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Nombre
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var usuario = await _context.tblUsuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.tblUsuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(GestionUsuario));
        }
        public IActionResult IndexUser()
        {
            return View();
        }


        [HttpGet]
        public IActionResult CreateUser()
        {
            var model = new CreateUserViewModel
            {
                RolesDisponibles = _context.tblRoles.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Nombre
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel Uvm)
        {
            ModelState.Remove("RolesDisponibles"); 
            if (ModelState.IsValid)
            {
                Usuario U = new Usuario();
                U.Nombre = Uvm.Nombre;
                U.Correo = Uvm.Email;
                U.IdRol = Uvm.IdRolSeleccionado;

                CreatePasswordHash(Uvm.Password, out byte[] passwordHash, out byte[] passwordSalt);
                U.PasswordHash = passwordHash;
                U.PasswordSalt = passwordSalt;

                _context.tblUsuarios.Add(U);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GestionUsuario));
            }
            else
            {
                Uvm.RolesDisponibles = _context.tblRoles.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Nombre
                }).ToList();
                foreach (var item in ModelState)
                {
                    Console.WriteLine($"Key: {item.Key}, Errors: {string.Join(", ", item.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return View(Uvm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RegisterFromRequest(int id)
        {
            var usuarioTemporal = await _context.tblUsuariosTemporales.FindAsync(id);
            if (usuarioTemporal == null)
            {
                return NotFound();
            }

            var model = new CreateUserViewModel
            {
                Nombre = usuarioTemporal.Nombre,
                Email = usuarioTemporal.Correo,
                IdRolSeleccionado = _context.tblRoles.Where(r => r.Nombre == usuarioTemporal.Rol).Select(r => r.Id).FirstOrDefault(),
                RolesDisponibles = _context.tblRoles.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Nombre
                }).ToList()
            };

            return View("CreateUser", model); // reutiliza la vista CreateUser
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
    }
}