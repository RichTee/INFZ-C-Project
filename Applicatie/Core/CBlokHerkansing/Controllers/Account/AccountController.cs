using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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

        // TODO: Resolve having to click login twice to logout, redirect properly when user is logged in.
        public ActionResult Login()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel viewModel, String returnUrl)
        {

            if (ModelState.IsValid)
            {
                bool auth = accountDBController.isAuthorized(viewModel.UserName, viewModel.PassWord);

                if (auth)
                {
                    FormsAuthentication.SetAuthCookie(viewModel.UserName, false);
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        if (User.IsInRole("MEDEWERKER"))
                            return RedirectToAction("Beheer");
                        else
                            return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("loginfout", "Username en Password zijn incorrect");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        
        // Is this needed if we let Login handle both?
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return RedirectToAction("Index", "HomeController");
        }

        public ActionResult Register()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
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
        
        // Should AccountController handle profiles?
        public ActionResult Profile()
        {
            return View();
        }

        // Should AccountController handle CRUD profile(CMS)?
        public ActionResult Beheer()
        {
            return View();
        }
    }
}
