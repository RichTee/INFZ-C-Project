using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CBlokHerkansing.Models.Account;
using CBlokHerkansing.Controllers.Database;
using CBlokHerkansing.Authorisation;
using CBlokHerkansing.ViewModels.Account;
using CBlokHerkansing.Models.Product;
using CBlokHerkansing.ViewModels.Product;

namespace CBlokHerkansing.Controllers
{
    public class AccountController : Controller
    {
        // TODO: protected > private
        // Reason: All Database activities are within the same namespace, saves the creation of new controllers and reuses old ones.
        // Space Efficient, Speed efficient, Resource efficient.
        private AccountDBController accountDBController = new AccountDBController();
        private KlantDBController klantDBController = new KlantDBController();
        private ProductDBController productDBController = new ProductDBController();
        private AanbiedingDBController aanbiedingDBController = new AanbiedingDBController();

        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Account"); // Ternary Operator when already logged in to redirect to somewhere else.
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

        /*
         * 
         * Alle Klant wijzigingen naar eigen controller KlantController.cs ?
         * 
         */

        // Should AccountController handle profiles?
        [CustomUnauthorized(Roles="KLANT")]
        public ActionResult Profiel()
        {
            string usr = User.Identity.Name;

            Klant klantGegevens = klantDBController.GetKlantInformatie(usr);

            if (klantGegevens == null)
            {
                // ViewBag error message
            }

            KlantViewModel viewModel = new KlantViewModel();
            viewModel.klantOverzicht = klantGegevens;

            return View(viewModel);
        }

        [CustomUnauthorized(Roles = "KLANT, ADMIN")]
        public ActionResult WijzigKlant(string email)
        {
            if (!User.IsInRole("ADMIN") && !User.Identity.Name.Equals(email))
                return View();
            try
            {
                Klant klant = klantDBController.GetKlantInformatie(email);
                return View(klant);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }
        }

        [HttpPost]
        [CustomUnauthorized(Roles = "KLANT, ADMIN")]
        public ActionResult WijzigKlant(Klant klant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    klantDBController.UpdateKlant(klant);

                    return RedirectToAction("Index", "Home"); // TODO: Redirect naar Beheer of Profiel, niet index.
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                    return View();
                }
            }
            else
            {
                return View(klant);
            }
        }

        /*
         * 
         * Alle CRUD aanbiedingen naar AanbiedingController?
         * 
         */
        

        /*
         * 
         * Should AccountController handle CRUD profile(CMS) and all of the belonging functions?
         * 
         */
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult Beheer()
        {
            List<Klant> klanten = klantDBController.GetKlanten();
            List<ProductBase> producten = productDBController.GetProducten();
            List<ProductAanbieding> aanbiedingen = aanbiedingDBController.GetAanbiedingen();

            BeheerderViewModel viewModel = new BeheerderViewModel();
            viewModel.klantOverzicht = klanten;
            viewModel.productBaseOverzicht = producten;
            viewModel.productAanbiedingOverzicht = aanbiedingen;

            return View(viewModel);
        }
    }
}
