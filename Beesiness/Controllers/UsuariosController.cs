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

        [HttpGet]
        public IActionResult GestionUsuario()
        {
            var model = new UserListViewModel
            {
                Usuarios = _context.tblUsuarios.Include(u => u.Rol).ToList()
            };
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
            if (ModelState.IsValid)
            {
                var usuario = await _context.tblUsuarios.FindAsync(model.Id);
                if (usuario == null)
                {
                    return NotFound();
                }

                // Actualizar los campos
                usuario.Nombre = model.Nombre;
                usuario.Correo = model.Correo;
                usuario.IdRol = model.IdRolSeleccionado;

                // Si se ingresó una nueva contraseña, actualizarla.
                if (!string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    // Aquí debes usar el mismo mecanismo que utilizaste para crear la contraseña original
                    // Esto es solo un ejemplo y deberás reemplazarlo por tu lógica de hashing
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

            // Si el modelo no es válido, recarga la vista con la información existente
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

            // Añadir validación si es necesario para evitar que un usuario se elimine a sí mismo
            // Por ejemplo:
            // if (User.Identity.Name == usuario.Correo) {
            //     // Mostrar mensaje de error o redirigir
            // }

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
            ModelState.Remove("RolesDisponibles"); //Esto para que no pase el RolesDisponibles de CreateViewMoedl
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
                return RedirectToAction(nameof(Index));
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
                // Pre-rellena el Rol basado en el texto, esto supone que tienes una correspondencia entre el texto del rol y el IdRol.
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