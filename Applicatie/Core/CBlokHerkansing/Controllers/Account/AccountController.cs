using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBlokHerkansing.Models.Account;
using CBlokHerkansing.Controllers.Database;

namespace CBlokHerkansing.Controllers
{
    public class AccountController : Controller
    {
        // TODO: protected > private
        // Reason: All Database activities are within the same namespace, saves the creation of new controllers and reuses old ones.
        // Space Efficient, Speed efficient, Resource efficient.
        private AccountDBController accountDBController = new AccountDBController();

        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Login", "AccountController"); // Ternary Operator when already logged in to redirect to somewhere else.
        }

        public ActionResult Login()
        {
            return View();
        }
        
        public ActionResult Logout()
        {
            return RedirectToAction("Index", "HomeController");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistrationModel registrationModel)
        {
            if (ModelState.IsValid)
            {
                if (accountDBController.checkGebruikerDuplicaat(registrationModel.Username))
                {
                    accountDBController.InsertKlant(registrationModel);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("registratieFout", "Gebruikersnaam bestaat al");
                    return View();
                }

            }
            else
            {
                ModelState.AddModelError("registratieFout", "Een of meerdere ingevoerde gegevens voldoen niet aan onze eisen");
                return View();
            }
        }
    }
}