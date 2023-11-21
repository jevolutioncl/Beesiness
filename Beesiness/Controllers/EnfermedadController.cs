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
        public async Task<IActionResult> EnfermedadIndex(string filtro)
        {
            if (User.Identity.IsAuthenticated)
            {
                var variable1 = await _context.tblEnfermedades.ToListAsync();
                if (filtro != null)
                {
                    variable1 = await _context.tblEnfermedades
                        .Where(d => d.Nombre.Contains(filtro))
                        .ToListAsync();
                }
                return View(variable1);
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
