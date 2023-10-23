using Beesiness.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Beesiness.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        { _context = context; }

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
                    //Usuario no encontrado
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
