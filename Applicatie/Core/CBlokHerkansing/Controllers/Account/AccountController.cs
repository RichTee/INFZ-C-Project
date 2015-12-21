using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CBlokHerkansing.Models.Account;
using CBlokHerkansing.Controllers.Database;
using CBlokHerkansing.Authorisation;

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
            return RedirectToAction("Index", "Home"); // Ternary Operator when already logged in to redirect to somewhere else.
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
                bool auth = accountDBController.isAuthorized(viewModel.Email, viewModel.PassWord);

                if (auth)
                {
                    FormsAuthentication.SetAuthCookie(viewModel.Email, false);
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        if (User.IsInRole("ADMIN"))
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
            
            //TODO: Make a small page that notifies the user being logged out or make a small notification on the index page.
            return RedirectToAction("Index", "Home");
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
                if (accountDBController.checkGebruikerDuplicaat(registrationModel.Email))
                {
                    accountDBController.InsertKlant(registrationModel);
                    // TODO: User melden dat email al in gebruik is via een ViewBag message.
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
        [CustomUnauthorized(Roles="KLANT")]
        public ActionResult Profiel()
        {
            return View();
        }

        // Should AccountController handle CRUD profile(CMS)?
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult Beheer()
        {
            return View();
        }
    }
}
