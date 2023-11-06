using Beesiness.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;

namespace Beesiness.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
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