using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fatec_Facilities.Models;
using Microsoft.AspNetCore.Authorization;

namespace Fatec_Facilities.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Contexto _contexto;

        private readonly EmailSenderController _emailSenderController;

        public HomeController(ILogger<HomeController> logger, Contexto context)
        {
            _contexto = context;

            _logger = logger;

            _emailSenderController = new EmailSenderController(_contexto);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SendContactEmail(String name, String emailToReply, String subject, String message){

            EmailSenderController emailSender = new EmailSenderController(_contexto);
            EmailSender emailContact = new EmailSender();

            emailContact.Destinatario = "imperiumfacilities@gmail.com";

            emailContact.Titulo = subject;
            emailContact.Corpo = message;
            emailContact.Remetente = emailToReply;
            //emailContact.

            _emailSenderController.sendEmail(emailContact);
 
            return View("EmailSent");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
