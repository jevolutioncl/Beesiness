using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Security.Permissions;

namespace Beesiness.Controllers
{
    public class RolController : Controller
    {
        private readonly AppDbContext _context; 

        public RolController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult RolGeneral()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("LoginIn", "Auth");
        }
        private const int PageSize = 6;
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RolIndex(string searchString, string filterType, int pageNumber = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["CurrentFilter"] = searchString;

                var roles = from u in _context.tblRoles
                            select u;
                if (!String.IsNullOrEmpty(searchString))
                {
                    switch (filterType)
                    {
                        case "Nombre":
                            roles = roles.Where(u => u.Nombre.Contains(searchString));
                            break;
                        case "Descripcion":
                            roles = roles.Where(u => u.Descripcion.Contains(searchString));
                            break;
                    }    
                }

                int totalRoles = await roles.CountAsync();
                int totalPages = (int)Math.Ceiling(totalRoles / (double)PageSize);

                var pagedRoles = await roles
                        .Skip((pageNumber - 1) * PageSize)
                        .Take(PageSize)
                        .ToListAsync();
                var model = new RolesIndexViewModel
                {
                    Roles = pagedRoles,
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling(totalRoles / (double)PageSize)
                };

                if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_RolListPartial", model);
                }

                return View(model);
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        public IActionResult RolCrear() 
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        public async Task<IActionResult> RolEditar(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return View(); }
                else
                {
                    var model = await _context.tblRoles.FirstOrDefaultAsync(x => x.Id == id);
                    return View(model);
                }                
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        public async Task<IActionResult> RolBorrar(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return NotFound(); }
                else
                {
                    var model = await _context.tblRoles.FirstOrDefaultAsync(x => x.Id == id);
                    if (model == null) { return NotFound(); }
                    else
                    {
                        _context.Remove(model);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("RolIndex");
                    }                    
                }
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        [HttpPost]
        public async Task<IActionResult> RolCrear(Rol rol)
        {
            if (User.Identity.IsAuthenticated)
            {
                ModelState.Remove("Usuarios");
                if (ModelState.IsValid)
                {
                    _context.Add(rol);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("RolIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Guardar");
                    return View(rol);
                }
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        [HttpPost]
        public async Task<IActionResult> RolEditar(Rol rol)
        {            
            if (User.Identity.IsAuthenticated)
            {
                ModelState.Remove("Usuarios");
                if (ModelState.IsValid)
                {
                    _context.Update(rol);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("RolIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Editar");
                    return View(rol);
                }
            }
            return RedirectToAction("LoginIn", "Auth");
        }
    }
}
