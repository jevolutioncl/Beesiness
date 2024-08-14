using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Beesiness.Models
{
    /// <summary>
    /// Servicio para interactuar con el asistente ChatGPT.
    /// Este asistente está diseñado para ayudar a los usuarios con preguntas relacionadas con la apicultura.
    /// </summary>
    public class ChatGptService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ChatGptService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<List<MensajeAI>> AskChatGptAsync(List<MensajeAI> mensajes, string userMessage)
        {
            // Validar que la lista no sea null
            if (mensajes == null)
            {
                mensajes = new List<MensajeAI>();
            }

            mensajes.Add(new MensajeAI { Role = "user", Content = userMessage });

            if (mensajes.Count == 1)
            {
                mensajes.Insert(0, new MensajeAI { Role = "system", Content = "Eres un asistente de apicultura, debes ayudar a responder preguntas sobre apicultura." +
                    "De nada más, en el caso que el usuario pida otra cosa, tú solo responde cosas de apicultura." });
            }

            var data = new
            {
                model = "gpt-4o-mini",
                messages = mensajes.Select(m => new { role = m.Role, content = m.Content }).ToArray()
            };

            HttpResponseMessage response;
            try
            {
                string requestUri = "https://api.openai.com/v1/chat/completions";
                response = await _httpClient.PostAsJsonAsync(requestUri, data);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error calling API. StatusCode={response.StatusCode}, Response={errorResponse}");
                return null;
            }

            var responseContent = await response.Content.ReadFromJsonAsync<OpenAiChatResponse>();
            var assistantMessage = responseContent?.Choices?.FirstOrDefault()?.Message?.Content;

            if (!string.IsNullOrEmpty(assistantMessage))
            {
                mensajes.Add(new MensajeAI { Role = "assistant", Content = assistantMessage });
            }

            return mensajes;
        }
    }

    public class OpenAiChatResponse
    {
        public List<ChatChoice> Choices { get; set; }
    }

    public class ChatChoice
    {
        public MensajeAI Message { get; set; }
    }
}
