using System.Web.Mvc;
using MyStoreDomain.Concrete;
using MyStoreDomain.Entities;

namespace MyStoreWebUI.Controllers
{   //bkbkbkbkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk
    public class AboutMeController : Controller
    {
        // GET: AboutMe
        public ActionResult AboutME()
        {
            return View();
        }
        public ActionResult WatIOffer()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Contact(MessageToMe message)
        {
            if (ModelState.IsValid)
            {
                EmailOrderProcessor email = new EmailOrderProcessor(new EmailSettings());

                email.ContactWotchMe(message);

                return View("Dziekuje");
            }
            else
            {
                return View("Contact");
            }
        }
        [HttpGet]
        public ActionResult CV()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CV(MessageToMe message)
        {
            if (message.Email != null)
            {
                EmailOrderProcessor email = new EmailOrderProcessor(new EmailSettings());
                email.ContactWotchMe(message);
                return View("Dziekuje");
            }
            else
            {
                return View("CV");
            }
        }

        public ActionResult Code()
        {
            return View();
        }
    }
}