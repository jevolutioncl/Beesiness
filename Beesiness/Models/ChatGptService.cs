using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
        private readonly AppDbContext _dbContext;

        public ChatGptService(HttpClient httpClient, IConfiguration configuration, AppDbContext dbContext)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _dbContext = dbContext;
        }

        public async Task<List<MensajeAI>> AskChatGptAsync(List<MensajeAI> mensajes, string userMessage)
        {
            // Validar que la lista no sea null
            if (mensajes == null)
            {
                mensajes = new List<MensajeAI>();
            }

            // Agregar el mensaje del usuario
            mensajes.Add(new MensajeAI { Role = "user", Content = userMessage });

            // Asegurar el mensaje de sistema se agrega solo una vez
            if (mensajes.Count == 1)
            {
                mensajes.Insert(0, new MensajeAI
                {
                    Role = "system",
                    Content = "Eres un asistente de apicultura, debes ayudar a responder preguntas sobre apicultura." +
                    "Si el usuario pide información sobre un usuario específico, debes buscar en la base de datos." +
                    "Utiliza comandos como [BuscarUsuarioPorNombre], [BuscarUsuarioPorCorreo], [BuscarUsuarioPorRol]."
                });
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
                Console.WriteLine("Enviando solicitud a OpenAI...");
                response = await _httpClient.PostAsJsonAsync(requestUri, data);
                Console.WriteLine("Solicitud enviada.");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return mensajes;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error calling API. StatusCode={response.StatusCode}, Response={errorResponse}");
                return mensajes;
            }

            var responseContent = await response.Content.ReadFromJsonAsync<OpenAiChatResponse>();
            var assistantMessage = responseContent?.Choices?.FirstOrDefault()?.Message?.Content;

            Console.WriteLine($"assistantMessage: {assistantMessage}");  // Depuración para ver el mensaje recibido

            if (!string.IsNullOrEmpty(assistantMessage))
            {
                Console.WriteLine($"assistantMessage recibido: {assistantMessage}");  // Depuración adicional

                if (assistantMessage.Contains("[BuscarUsuarioPorNombre]"))
                {
                    var nombre = ExtractSearchParameter(assistantMessage, "[BuscarUsuarioPorNombre]");
                    Console.WriteLine($"Nombre extraído: {nombre}");  // Depuración adicional para verificar la extracción

                    var userInfo = await GetUserByNombreAsync(nombre);
                    Console.WriteLine($"Resultado búsqueda por nombre: {userInfo}");  // Depuración adicional para el resultado
                    mensajes.Add(new MensajeAI { Role = "assistant", Content = userInfo });
                }
                else if (assistantMessage.Contains("[BuscarUsuarioPorCorreo]"))
                {
                    var correo = ExtractSearchParameter(assistantMessage, "[BuscarUsuarioPorCorreo]");
                    var userInfo = await GetUserByCorreoAsync(correo);
                    mensajes.Add(new MensajeAI { Role = "assistant", Content = userInfo });
                }
                else if (assistantMessage.Contains("[BuscarUsuarioPorRol]"))
                {
                    var rol = ExtractSearchParameter(assistantMessage, "[BuscarUsuarioPorRol]");
                    var userInfo = await GetUserByRolAsync(rol);
                    mensajes.Add(new MensajeAI { Role = "assistant", Content = userInfo });
                }
                else
                {
                    // Si no hay comandos específicos, simplemente agrega el mensaje a la lista
                    mensajes.Add(new MensajeAI { Role = "assistant", Content = assistantMessage });
                }
            }
            else
            {
                Console.WriteLine("assistantMessage es null o vacío");  // Depuración para casos donde el mensaje sea null
                mensajes.Add(new MensajeAI { Role = "assistant", Content = "Lo siento, no pude entender tu solicitud. ¿Podrías intentar de nuevo?" });
            }


            return mensajes;
        }


        private string ExtractSearchParameter(string gptResponse, string command)
        {
            var startIndex = gptResponse.IndexOf(command) + command.Length;

            if (startIndex >= gptResponse.Length)
                return string.Empty;  // Manejo de caso donde no se puede extraer el parámetro

            var parameter = gptResponse.Substring(startIndex).Trim();

            // Remover comillas si están presentes
            parameter = parameter.Trim('"');

            Console.WriteLine($"Parameter extracted: '{parameter}'");  // Línea de depuración para ver el parámetro extraído

            return parameter;
        }


        // Métodos para realizar las consultas en la base de datos usando Entity Framework
        private async Task<string> GetUserByNombreAsync(string nombre)
        {
            Console.WriteLine($"Buscando usuario con el nombre: '{nombre}'");  // Línea de depuración

            var usuarios = await _dbContext.tblUsuarios
                                           .Include(u => u.Rol)
                                           .Where(u => u.Nombre.Contains(nombre))
                                           .ToListAsync();

            if (usuarios.Any())
            {
                var usuario = usuarios.First();  // Supongamos que solo queremos mostrar el primero
                Console.WriteLine($"Usuario encontrado: Nombre: {usuario.Nombre}, Correo: {usuario.Correo}, Rol: {usuario.Rol.Nombre}");
                return $"Claro, encontré a {usuario.Nombre} que tiene el correo {usuario.Correo} y su rol es {usuario.Rol.Nombre}.";
            }
            else
            {
                Console.WriteLine("No se encontraron usuarios con ese nombre.");
                return "No se encontraron usuarios con ese nombre.";
            }
        }


        private async Task<string> GetUserByCorreoAsync(string correo)
        {
            var usuario = await _dbContext.tblUsuarios
                                          .Include(u => u.Rol)
                                          .FirstOrDefaultAsync(u => u.Correo.Contains(correo));

            if (usuario != null)
            {
                return $"ID: {usuario.Id}, Nombre: {usuario.Nombre}, Correo: {usuario.Correo}, Rol: {usuario.Rol.Nombre}";
            }
            else
            {
                return "No se encontró un usuario con ese correo.";
            }
        }

        private async Task<string> GetUserByRolAsync(string rolNombre)
        {
            Console.WriteLine($"Buscando usuarios con el rol: {rolNombre}");  // Línea de depuración

            var usuarios = await _dbContext.tblUsuarios
                                           .Include(u => u.Rol)
                                           .Where(u => u.Rol.Nombre.Contains(rolNombre))
                                           .ToListAsync();

            if (usuarios.Any())
            {
                return string.Join("\n", usuarios.Select(u => $"ID: {u.Id}, Nombre: {u.Nombre}, Correo: {u.Correo}, Rol: {u.Rol.Nombre}"));
            }
            else
            {
                return "No se encontraron usuarios con ese rol.";
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
}
