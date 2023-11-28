using Beesiness.Models;
using Humanizer;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Management;

namespace Beesiness.Controllers
{
    public class AgendaController : Controller
    {
        private readonly AppDbContext _context;
        
        public static List<TareaViewModel> Agenda { get; set; } = new List<TareaViewModel>();

        public AgendaController(AppDbContext context)
        {
            _context = context;
        }                

        public async Task<IActionResult> Iniciar() 
        {               
            //llenamos los datos para nuestra agenda
            var tareas = await _context.tblTareas.
                Where(x => x.Status == "Pendiente").ToListAsync();

            foreach (var tarea in tareas)
            {
                var tvm = new TareaViewModel();
                tvm.Id = tarea.Id;
                tvm.Nombre = tarea.Nombre;
                tvm.Descripcion = tarea.Descripcion;
                tvm.CorreoAviso = tarea.CorreoAviso;
                tvm.FechaRegistro = tarea.FechaRegistro;
                tvm.FechaRegistro = tarea.FechaRealizacion;
                tvm.Status = tarea.Status;

                var colmenasTarea = await _context.tblTareaColmena.
                Where(x => x.IdTarea == tarea.Id).ToListAsync();
                
                foreach (var item in colmenasTarea)
                {
                    int numid = 0;
                    var col = await _context.tblColmenas.FirstOrDefaultAsync(x => x.Id == item.IdColmena);
                    if (col != null) numid = col.numIdentificador; 
                    var tcolvm = new TareaColViewModel 
                    {
                        Id = item.IdColmena,
                        idTarea = item.IdTarea,
                        idColmena = item.IdColmena,
                        numeroColmena = numid
                    }; 
                    tvm.ColmenasTarea.Add(tcolvm);                    
                }
                Agenda.Add(tvm);
            }

            //StartAsync(CancellationToken.None);
            return RedirectToAction("LoginIn", "Auth");
        }

        [HttpPost]
        public IActionResult EnviarEmail() 
        {
            //Con esto me evito tener que crear otro modelo adicional para enviar el correo
            string destinatario = Request.Form["destinatario"];
            string asunto = Request.Form["asunto"];
            string cuerpo = Request.Form["cuerpo"];

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("dev19811471@gmail.com"));
            //email.From.Add(MailboxAddress.Parse("simeon33@ethereal.email"));
            //email.To.Add(MailboxAddress.Parse("ashley.nienow69@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(destinatario));
            email.Subject = asunto;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = cuerpo };

            using var smtp = new SmtpClient();
            //smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            //smtp.Authenticate("simeon33@ethereal.email", "qzBuZJ1RrPzTKBqZNq");
            smtp.Authenticate("dev19811471@gmail.com", "oiia gmnl yvqa raym");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok("Correo enviado"); //eliminar esto despues
        }
        
    }
}
