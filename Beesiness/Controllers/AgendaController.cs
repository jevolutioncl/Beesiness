﻿using Humanizer;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Management;

namespace Beesiness.Controllers
{
    public class AgendaController : Controller
    {
        public IActionResult AgendaIndex()
        {
            return View();
        }       

        [HttpPost]
        public IActionResult EnviarEmail() 
        {
            //Con esto me evito tener que crear otro modelo adicional para enviar el correo
            string destinatario = Request.Form["destinatario"];
            string asunto = Request.Form["asunto"];
            string cuerpo = Request.Form["cuerpo"];

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("simeon33@ethereal.email"));
            //email.To.Add(MailboxAddress.Parse("ashley.nienow69@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(destinatario));
            email.Subject = asunto;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = cuerpo };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("simeon33@ethereal.email", "qzBuZJ1RrPzTKBqZNq");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok("Correo enviado");
        }

    }
}