using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beesiness.Controllers
{
    public class ColmenaController : Controller
    {
        private readonly AppDbContext _context;

        public ColmenaController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ColmenaIndex(string filtro)
        {
            if (User.Identity.IsAuthenticated)
            {
                var colmenas = await _context.tblColmenas.ToListAsync();
                if (filtro != null)
                {
                    colmenas = await _context.tblColmenas
                        .Where(d => d.TipoColmena.Contains(filtro))
                        .ToListAsync();
                }
                return View(colmenas);
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public IActionResult ColmenaCrear()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        public async Task<IActionResult> ColmenaEditar(int colmenaId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (colmenaId == 0) { return View(); } //en vez de hacer un return view podriamos mandar un mensaje o volver al index
                else
                {
                    var colmena = await _context.tblColmenas.FirstOrDefaultAsync(x => x.Id == colmenaId);
                    return View(colmena);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public async Task<IActionResult> ColmenaBorrar(int colmenaId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (colmenaId == 0) { return NotFound(); }
                else
                {
                    var colmena = await _context.tblColmenas.FirstOrDefaultAsync(x => x.Id == colmenaId);
                    if (colmena == null) { return NotFound(); }
                    else
                    {
                        _context.Remove(colmena);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("ColmenaIndex");
                    }

                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }


        [HttpPost]
        public async Task<IActionResult> ColmenaCrear(Colmena colmena)
        {
            if (User.Identity.IsAuthenticated)
            {                
                if (ModelState.IsValid)
                {                    
                    _context.Add(colmena);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ColmenaIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Guardar");
                    return View(colmena);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        [HttpPost]
        public async Task<IActionResult> ColmenaEditar(Colmena colmena)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    _context.Update(colmena);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ColmenaIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Editar");
                    return View(colmena);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

    }
}
