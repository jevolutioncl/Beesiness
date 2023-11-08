using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beesiness.Controllers
{
    public class EnfermedadColmenaController : Controller
    {
        private readonly AppDbContext _context;

        public EnfermedadColmenaController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> EnfermedadColmenaIndex(DateTime filtro)
        {
            if (User.Identity.IsAuthenticated)
            {
                var variable1 = await _context.tblEnfermedadColmena.ToListAsync();
                if (filtro != null)
                {
                    variable1 = await _context.tblEnfermedadColmena
                        .Where(d => d.FechaDeteccion.Equals(filtro))
                        .ToListAsync();
                }
                return View(variable1);
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public IActionResult EnfermedadColmenaCrear()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        public async Task<IActionResult> EnfermedadColmenaEditar(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return View(); }
                else
                {
                    var variable1 = await _context.tblEnfermedadColmena.FirstOrDefaultAsync(x => x.Id == id);
                    return View(variable1);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public async Task<IActionResult> EnfermedadColmenaBorrar(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return NotFound(); }
                else
                {
                    var variable1 = await _context.tblEnfermedadColmena.FirstOrDefaultAsync(x => x.Id == id);
                    if (variable1 == null) { return NotFound(); }
                    else
                    {
                        _context.Remove(variable1);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("EnfermedadColmenaIndex");
                    }

                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        [HttpPost]
        public async Task<IActionResult> EnfermedadColmenaCrear(EnfermedadColmena enfermedadColmena)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(enfermedadColmena);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("EnfermedadColmenaIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Guardar");
                    return View(enfermedadColmena);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        [HttpPost]
        public async Task<IActionResult> EnfermedadColmenaEditar(EnfermedadColmena enfermedadColmena)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    _context.Update(enfermedadColmena);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("EnfermedadColmenaIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Editar");
                    return View(enfermedadColmena);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }
    }
}
