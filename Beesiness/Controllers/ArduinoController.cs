using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beesiness.Controllers
{
    public class ArduinoController : Controller
    {
        private readonly AppDbContext _context;

        public ArduinoController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> EstadoArduino()
        {
            var estadoArduino = await _context.tblEstadoArduino
                                              .OrderByDescending(e => e.UltimaComunicacion)
                                              .FirstOrDefaultAsync();

            if (estadoArduino == null)
            {
                return View(new EstadoArduino { ArduinoConectado = false });
            }

            return View(estadoArduino);
        }
        [HttpGet]
        [Route("VerificarEstadoArduino")] 
        public async Task<IActionResult> VerificarEstadoArduino()
        {
            var estadoActual = await _context.tblEstadoArduino
                                             .OrderByDescending(e => e.UltimaComunicacion)
                                             .FirstOrDefaultAsync();

            if (estadoActual == null)
            {
                
                return Json(new { estaConectado = false });
            }

            return Json(new { estaConectado = estadoActual.ArduinoConectado });
        }
    }
}
