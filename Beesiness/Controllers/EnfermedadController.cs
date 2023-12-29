using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beesiness.Controllers
{
    public class EnfermedadController : Controller
    {
        private readonly AppDbContext _context;

        public EnfermedadController(AppDbContext context)
        {
            _context = context;
        }
        private const int PageSize = 6;

        public async Task<IActionResult> EnfermedadIndex(string searchString, string filterType, int pageNumber = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["CurrentFilter"] = searchString;

                var enfermedad = from u in _context.tblEnfermedades select u;
                if(!String.IsNullOrEmpty(searchString))
                {
                    switch (filterType)
                    {
                        case "Nombre":
                            enfermedad = enfermedad.Where(u => u.Nombre.Contains(searchString));
                            break;
                        case "Descripcion":
                            enfermedad = enfermedad.Where(u => u.Descripcion.Contains(searchString));
                            break;
                    }
                }

                int totalEnfermedades = await enfermedad.CountAsync();
                int totalPages = (int)Math.Ceiling(totalEnfermedades / (double)PageSize);

                var pagedEnfermedades = await enfermedad
                            .Skip((pageNumber - 1) * PageSize)
                            .Take(PageSize)
                            .ToListAsync();
                var model = new EnfermedadListViewModel
                {
                    Enfermedades = pagedEnfermedades,
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling(totalEnfermedades / (double)PageSize)
                };

                if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_UserListPartial", model);
                }
                return View(model);
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public IActionResult EnfermedadCrear()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        public IActionResult EnfermedadGeneral()
        {
            return View("EnfermedadGeneral");
        }

        public async Task<IActionResult> EnfermedadEditar(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return View(); }
                else
                {
                    var variable1 = await _context.tblEnfermedades.FirstOrDefaultAsync(x => x.Id == id);
                    return View(variable1);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public async Task<IActionResult> EnfermedadBorrar(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return NotFound(); }
                else
                {
                    var variable1 = await _context.tblEnfermedades.FirstOrDefaultAsync(x => x.Id == id);
                    if (variable1 == null) { return NotFound(); }
                    else
                    {
                        _context.Remove(variable1);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("EnfermedadIndex");
                    }

                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        [HttpPost]
        public async Task<IActionResult> EnfermedadCrear(Enfermedad enfermedad)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(enfermedad);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("EnfermedadIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Guardar");
                    return View(enfermedad);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        [HttpPost]
        public async Task<IActionResult> EnfermedadEditar(Enfermedad enfermedad)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    _context.Update(enfermedad);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("EnfermedadIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Editar");
                    return View(enfermedad);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }
    }
}
