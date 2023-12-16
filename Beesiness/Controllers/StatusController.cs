using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Beesiness.Controllers
{
    public class StatusController : Controller
    {
        private readonly ILogger<StatusController> _logger;

        public StatusController(ILogger<StatusController> logger)
        {
            _logger = logger;
        }

        public IActionResult EstadoSistema()
        {
            // No necesitas realizar llamadas asíncronas aquí ya que los datos se obtendrán a través de la API
            return View();
        }
    }
}