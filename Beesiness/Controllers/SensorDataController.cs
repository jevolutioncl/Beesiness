using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beesiness.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SensorDataController : Controller
    {
        private readonly AppDbContext _context;

        public SensorDataController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SensorData data)
        {
            var infoSensor = new InfoSensores
            {
                Fecha = DateTime.Now,
                Temperatura = data.Temperatura,
                IdColmena = data.IdColmena
            };

            _context.tblInfoSensores.Add(infoSensor);
            await _context.SaveChangesAsync();
            // Añade un registro para confirmar la inserción
            Console.WriteLine($"Registro guardado: {infoSensor.Id}");
            return Ok(infoSensor.Id);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var datos = await _context.tblInfoSensores.OrderByDescending(x => x.Fecha).ToListAsync();
            return Ok(datos);
        }

        [HttpGet("UltimaTemperatura/{idColmena}")]
        public async Task<IActionResult> GetUltimaTemperatura(int idColmena)
        {
            var ultimaTemperatura = await _context.tblInfoSensores
                .Where(x => x.IdColmena == idColmena)
                .OrderByDescending(x => x.Fecha)
                .FirstOrDefaultAsync();

            if (ultimaTemperatura == null)
            {
                return NotFound();
            }

            return Ok(ultimaTemperatura.Temperatura); // Devuelve solo la temperatura
        }

    }
}
public class SensorData
{
    public float Temperatura { get; set; }
    public int IdColmena { get; set; }
}