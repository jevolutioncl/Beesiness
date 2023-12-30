using System.Threading;
using System.Threading.Tasks;
using Beesiness;
using Beesiness.Controllers;
using Beesiness.Migrations;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.Smo;
using MimeKit;

namespace Beesiness.Models
{
    public class TareasSP : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly AppDbContext _context;


        public Task StartAsync(CancellationToken cancellationToken)
        {
            //_timer = new Timer(EnviarAvisos, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            _timer = new Timer(EnviarAvisos, null, TimeSpan.Zero, TimeSpan.FromSeconds(10)); //media hora
            return Task.CompletedTask;
        }

        public async void EnviarAvisos(object state)
        {
            Console.WriteLine("TareasSP");
            Console.WriteLine("Fecha y hora actual: " + DateTime.Now);
            Console.WriteLine(AgendaController.Agenda.Count);
            //string correo = ViewData["Correo"].ToString();

            if (AgendaController.Agenda != null)
            {
                foreach (var item in AgendaController.Agenda)
                {
                    DateTime fecha = item.FechaRealizacion.Value;

                    //primero eliminamos todos los elementos antiguos de Agenda
                    //AgendaController.Agenda.RemoveAll(x => x.FechaRegistro.Value < DateTime.Now.AddMinutes(60));

                    //solo para probar el codigo: Tareas antiguas = no mandamos mail
                    if (fecha < DateTime.Now.AddMinutes(-60) || fecha > DateTime.Now.AddMinutes(60))
                    {
                        Console.WriteLine("NO envio mail a esta tarea: " + item.Nombre + " con fecha: " + fecha);
                    }

                    if (fecha >= DateTime.Now.AddMinutes(-60) && fecha <= DateTime.Now.AddMinutes(60))
                    {
                        //necesito agregar el campo usuario a las tareas.
                        EnviarEmail(item.CorreoAviso.ToString(), item.Nombre.ToString(), item.Descripcion.ToString());
                        Console.WriteLine("SI envio mail a esta tarea: " + item.Nombre + " con fecha: " + fecha);
                    }
                }
            }

            //eliminamos las tareas antiguas despues solo para probar el codigo
            //eliminamos las incluso las tareas de una hora mas tarde porque ya mandamos los avisos
            AgendaController.Agenda.RemoveAll(x => x.FechaRealizacion.Value < DateTime.Now.AddMinutes(60));
            //una forma mas segura de eliminar seria agregar un statusAviso en las Tareas y luego
            //borro todas las tareas de Agenda con el status enviado.
            Console.WriteLine(AgendaController.Agenda.Count);
            Console.WriteLine("un ciclo de Enviar Avisos");
        }

        public void EnviarEmail(string destinatario, string asunto, string cuerpo)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("dev19811471@gmail.com"));
            //email.From.Add(MailboxAddress.Parse("simeon33@ethereal.email"));            
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
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}