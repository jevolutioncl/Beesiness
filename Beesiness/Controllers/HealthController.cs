using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Beesiness.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HealthController> _logger;

        public HealthController(AppDbContext context, ILogger<HealthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetSystemHealth")]
        public async Task<IActionResult> GetSystemHealth()
        {
            var healthStatus = new
            {
                Frontend = true, // Si este endpoint es alcanzado, el frontend está funcionando
                Backend = true, // Suponemos que si llegamos aquí el backend está ok
                Database = await TestDatabaseConnection(),
                Arduino = await TestArduinoConnection()
            };

            return Ok(healthStatus);
        }

        private async Task<bool> TestDatabaseConnection()
        {
            try
            {
                var count = await _context.tblColmenas.CountAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Database connection test failed: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> TestArduinoConnection()
        {
            try
            {
                var arduinoStatus = await _context.tblEstadoArduino.OrderByDescending(e => e.UltimaComunicacion).FirstOrDefaultAsync();
                return arduinoStatus?.ArduinoConectado ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Arduino connection test failed: {ex.Message}");
                return false;
            }
        }
    }
}
