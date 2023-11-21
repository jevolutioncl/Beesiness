using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Beesiness.Controllers
{
    public class MapaController : Controller
    {
        private readonly AppDbContext _context;
        public MapaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AgregarUbicacion(UbicacionMapa ubicacion)
        {
            if (ModelState.IsValid)
            {
                _context.tblUbicacionMapas.Add(ubicacion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ubicacion);
        }
    }
}
