using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Beesiness.DTOs;

namespace Beesiness.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SensorDataController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SensorDataController> _logger;  // ILogger

        public SensorDataController(AppDbContext context, ILogger<SensorDataController> logger)
        {
            _context = context;
            _logger = logger;
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
        [HttpPost("ActualizarEstadoArduino")]
        public async Task<IActionResult> ActualizarEstadoArduino([FromBody] EstadoArduinoDto estadoArduinoDto)
        {
            _logger.LogInformation("Endpoint ActualizarEstadoArduino llamado.");

            if (estadoArduinoDto == null)
            {
                _logger.LogWarning("El objeto estadoArduinoDto es null.");
                return BadRequest();
            }

            var colmenaExistente = await _context.tblColmenas.AnyAsync(c => c.Id == estadoArduinoDto.IdColmena);
            if (!colmenaExistente)
            {
                _logger.LogWarning($"No existe la colmena con ID: {estadoArduinoDto.IdColmena}");
                return BadRequest($"No existe la colmena con ID: {estadoArduinoDto.IdColmena}");
            }

            try
            {
                _logger.LogInformation($"Recibido: ArduinoConectado = {estadoArduinoDto.ArduinoConectado}, IdColmena = {estadoArduinoDto.IdColmena}");

                var estadoActual = await _context.tblEstadoArduino
                                                 .FirstOrDefaultAsync(e => e.ColmenaId == estadoArduinoDto.IdColmena);

                if (estadoActual == null)
                {
                    _logger.LogInformation("Creando nuevo registro de estado para la colmena.");
                    var nuevoEstado = new EstadoArduino
                    {
                        ArduinoConectado = estadoArduinoDto.ArduinoConectado,
                        UltimaComunicacion = DateTime.Now,
                        ColmenaId = estadoArduinoDto.IdColmena
                    };
                    _context.tblEstadoArduino.Add(nuevoEstado);
                }
                else
                {
                    _logger.LogInformation($"Actualizando estado existente para la colmena {estadoArduinoDto.IdColmena}.");
                    estadoActual.ArduinoConectado = estadoArduinoDto.ArduinoConectado;
                    estadoActual.UltimaComunicacion = DateTime.Now;
                    _context.Update(estadoActual);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Estado del Arduino actualizado con éxito.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el estado del Arduino: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }

}
public class SensorData
{
    public float Temperatura { get; set; }
    public int IdColmena { get; set; }
}
