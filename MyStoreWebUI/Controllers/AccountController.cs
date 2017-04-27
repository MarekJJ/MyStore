using System.Web.Mvc;
using MyStoreWebUI.Infastructure.Abstract;
using MyStoreWebUI.Models;

namespace MyStoreWebUI.Controllers
{
    public class AccountController : Controller
    {
           IAuthProvider authProvider; 

            public AccountController(IAuthProvider auth) //ninject Associates parameter with class FormsAuthProvider
        {
                authProvider = auth;
            }
            public ViewResult Login()
            {
                return View();
            }
            [HttpPost]
            public ActionResult Login(LoginViewModel model, string returnUrl)
            {
                if (ModelState.IsValid)
                {
                    if (authProvider.Authenticate(model.UserName, model.Password))
                    {
                        return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nieprawidłowa nazwa użytkownika lub niepoprawne hasło.");
                  return View();
                    }
                }
                else
                {
                    return View();
                }
            }
        
    }
}