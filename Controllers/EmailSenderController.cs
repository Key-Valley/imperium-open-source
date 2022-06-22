using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fatec_Facilities.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Fatec_Facilities.Controllers
{
    public class EmailSenderController : Controller
    {
        private readonly Contexto _contexto;

        public EmailSenderController(Contexto contexto)
        {
            _contexto = contexto;
        }

        // [Authorize]
        public Boolean sendEmail(EmailSender email)
        {
            try {

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(email.Remetente, email.Remetente));
                message.To.Add(new MailboxAddress(" ", email.Destinatario));
                message.Subject = email.Titulo;

                TextPart corpo = new TextPart("plain") { Text = email.Corpo };

                message.Body = corpo;

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("imperiumfacilities@gmail.com", "#1Mp3R1uM");
                    client.Send(message);
                    client.Disconnect(true);
                }

                return true;
            } 
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }

        public Boolean sendContactEmail(EmailSender email){
            try {
                email.Remetente = "Key Valley";

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(email.Remetente, email.EmailKeyValley));
                message.To.Add(new MailboxAddress(" ", email.Destinatario));
                message.ReplyTo.Add(new MailboxAddress(" ", email.ResponderPara));
                message.Subject = email.Titulo;

                TextPart corpo = new TextPart("plain") { Text = email.Corpo };

                message.Body = corpo;

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("keyvalleys.a@gmail.com", "$enha1518");
                    client.Send(message);
                    client.Disconnect(true);
                }

                return true;
            } 
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }

        // [Authorize]
        public void SaveToken(SaveToken savedToken)
        {
            savedToken.Data = DateTime.Now;
            _contexto.SaveToken.Add(savedToken);
            _contexto.SaveChanges();
        }
    }
}
