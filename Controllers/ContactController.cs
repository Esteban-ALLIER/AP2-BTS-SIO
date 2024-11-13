using System.Net;
using System.Net.Mail;
using ASPBookProject.ViewModels;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;

namespace ASPBookProject.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Faq()
        {
            return View();
        }

        [HttpGet]
        public  IActionResult Contact()
        {
                return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost, ActionName("Contact")]
        public IActionResult Contact(MailViewModel viewModel)
        {
            string sender = "bts.testperso@gmail.com";
            string pw = "ppig gflv vmwp ctve";
            string receiver = "esteallier@gmail.com";
            string messageComplet = viewModel.Nom + "\n" + viewModel.Prenom  + "\n" + viewModel.email
            + "\n" + viewModel.telephone   + "\n" + viewModel.message;
            MailMessage message = new MailMessage();
            message.From = new MailAddress(sender);
            message.Subject ="Nouveau Message";
            message.To.Add(receiver);
            message.Body= messageComplet;        

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(sender, pw),
                EnableSsl = true,
            };
            smtpClient.Send(message);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
