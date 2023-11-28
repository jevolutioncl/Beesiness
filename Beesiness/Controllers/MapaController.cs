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

        [HttpGet] 
        public IActionResult UbicacionCrear()
        {
            return View(new UbicacionMapa());
        }

        [HttpPost]
        public async Task<IActionResult> UbicacionCrear(UbicacionMapa ubicacion)
        {
            if (ModelState.IsValid)
            {
                _context.tblUbicacionMapas.Add(ubicacion);
                await _context.SaveChangesAsync();
                return RedirectToAction("MapaColmena","Colmena");
            }
            return View(ubicacion);
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerUbicacionesPredeterminadas()
        {
            var ubicaciones = await _context.tblUbicacionMapas.ToListAsync();
            return Json(ubicaciones);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarUbicacion(int id)
        {
            var ubicacion = await _context.tblUbicacionMapas.FindAsync(id);
            if (ubicacion == null)
            {
                return NotFound();
            }

            _context.tblUbicacionMapas.Remove(ubicacion);
            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
