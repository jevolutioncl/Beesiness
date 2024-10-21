﻿using Beesiness.Models;
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
        public async Task<IActionResult> AskApiAI(ChatViewModel model, string prompt)
        {
            if (string.IsNullOrEmpty(prompt))
            {
                model.Mensajes.Add(new MensajeAI { Role = "assistant", Content = "Por favor, escribe una pregunta o solicitud." });
                return View("Index", model);
            }

            if (model.Mensajes == null)
            {
                model.Mensajes = new List<MensajeAI>(); // Asegurarse de que la lista esté inicializada
            }

            model.Mensajes = await _chatGptService.AskChatGptAsync(model.Mensajes, prompt);
            return View("Index", model);
        }
    }
}