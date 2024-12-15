using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beesiness.Controllers
{
    public class ApiAIController : Controller
    {
        private readonly ChatGptService _chatGptService;

        public ApiAIController(ChatGptService chatGptService)
        {
            _chatGptService = chatGptService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new ChatViewModel();

            // Asegúrate de que la lista de mensajes esté inicializada
            if (model.Mensajes == null)
            {
                model.Mensajes = new List<MensajeAI>();
            }

            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> AskApiAI([FromBody] ChatViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Mensajes.LastOrDefault()?.Content))
            {
                return Json(new { messages = new List<MensajeAI> { new MensajeAI { Role = "assistant", Content = "Por favor, escribe una pregunta o solicitud." } } });
            }

            var prompt = model.Mensajes.LastOrDefault()?.Content;

            if (model.Mensajes == null)
            {
                model.Mensajes = new List<MensajeAI>(); // Inicializar la lista si es null
            }

            model.Mensajes = await _chatGptService.AskChatGptAsync(model.Mensajes, prompt);

            return Json(new { messages = model.Mensajes }); // Devuelve la lista actualizada
        }
    }
}
