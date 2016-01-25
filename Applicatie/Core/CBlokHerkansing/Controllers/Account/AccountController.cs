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
using CBlokHerkansing.Models.Klant;
using CBlokHerkansing.Models.Bestelling;
using CBlokHerkansing.Controllers.Manager;
using CBlokHerkansing.Models;

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
        private BestellingDBController bestellingDBController = new BestellingDBController();
        private CategorieDbController categorie = new CategorieDbController();

        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Account"); // Ternary Operator when already logged in to redirect to somewhere else.
        }

        public ActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
            }

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
                        if (User.IsInRole("MANAGER"))
                            return RedirectToAction("Manager");
                        else
                            return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.Foutmelding = "Uw email en/of wachtwoord is fout";
                    return View();
                }
            }
            else
            {
                ViewBag.Foutmelding = "Uw email en/of wachtwoord is fout";
                return View();
            }
        }
        
        // Is this needed if we let Login handle both?
        public ActionResult Logout()
        {
            if(User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
            }
            
            //TODO: Make a small page that notifies the user being logged out or make a small notification on the index page.
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            if(User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
            }

            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistrationModel registrationModel)
        {
            if (ModelState.IsValid)
            {
                if (!accountDBController.checkGebruikerDuplicaat(registrationModel.Email))
                {
                    accountDBController.InsertKlant(registrationModel);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("registratieFout", "email bestaat al");
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

            KlantBase klantGegevens = klantDBController.GetKlantInformatie(usr);
            List<BestelRegel> klantBestelling = bestellingDBController.GetBestelling(klantGegevens.Id);
            List<Adres> klantAdres = klantDBController.GetKlantAdressen(klantGegevens.Id);

            if (klantGegevens == null)
            {
                // ViewBag error message
            }

            KlantViewModel viewModel = new KlantViewModel();
            viewModel.klantOverzicht = klantGegevens;
            viewModel.bestellingOverzicht = klantBestelling;
            viewModel.adresOverzicht = klantAdres;

            // TempData Foutmelding
            if (TempData[Enum.ViewMessage.FOUTMELDING.ToString()] != null)
            {
                ViewBag.Foutmelding = TempData[Enum.ViewMessage.FOUTMELDING.ToString()];
                TempData.Remove(Enum.ViewMessage.FOUTMELDING.ToString());
            }

            // TempData Toevoeging
            if (TempData[Enum.ViewMessage.TOEVOEGING.ToString()] != null)
            {
                ViewBag.Wijziging = "U heeft " + TempData[Enum.ViewMessage.TOEVOEGING.ToString()] + " toegevoegt";
                TempData.Remove(Enum.ViewMessage.TOEVOEGING.ToString());
            }

            // TempData Wijziging
            if (TempData[Enum.ViewMessage.WIJZIGING.ToString()] != null)
            {
                ViewBag.Wijziging = "U heeft " + TempData[Enum.ViewMessage.WIJZIGING.ToString()] + " is gewijzigt";
                TempData.Remove(Enum.ViewMessage.WIJZIGING.ToString());
            }

            // TempData Verwijdering
            if (TempData[Enum.ViewMessage.VERWIJDERING.ToString()] != null)
            {
                ViewBag.Wijziging = "U heeft " + TempData[Enum.ViewMessage.VERWIJDERING.ToString()] + " verwijdert";
                TempData.Remove(Enum.ViewMessage.VERWIJDERING.ToString());
            }

            return View(viewModel);
        }

        /*
         * 
         * Should AccountController handle CRUD profile(CMS) and all of the belonging functions?
         * 
         */
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult Beheer()
        {
            List<KlantBase> klanten = klantDBController.GetKlanten();
            List<ProductBase> producten = productDBController.GetProducten(); // TODO: Remove, productDetail heeft productBase
            List<ProductDetail> productenDetail = productDBController.getProductenDetail();
            List<ProductAanbieding> aanbiedingen = aanbiedingDBController.GetAanbiedingen();
            List<BestelRegel> bestellingRegel = bestellingDBController.GetBestellingen();
            List<Categorie> categorieen = categorie.getListWithAllCategorieen();
            BeheerderViewModel viewModel = new BeheerderViewModel();
            viewModel.klantOverzicht = klanten;
            viewModel.productBaseOverzicht = producten;
            viewModel.productDetailOverzicht = productenDetail;
            viewModel.productAanbiedingOverzicht = aanbiedingen;
            viewModel.bestellingDetailOverzicht = bestellingRegel;
            viewModel.categorieenOverzicht = categorieen;

            // TempData Foutmelding
            if (TempData[Enum.ViewMessage.FOUTMELDING.ToString()] != null)
            {
                ViewBag.Foutmelding = TempData[Enum.ViewMessage.FOUTMELDING.ToString()];
                TempData.Remove(Enum.ViewMessage.FOUTMELDING.ToString());
            }

            // TempData Toevoeging
            if (TempData[Enum.ViewMessage.TOEVOEGING.ToString()] != null)
            {
                ViewBag.Wijziging = "U heeft " + TempData[Enum.ViewMessage.TOEVOEGING.ToString()] + " toegevoegt";
                TempData.Remove(Enum.ViewMessage.TOEVOEGING.ToString());
            }

            // TempData Wijziging
            if (TempData[Enum.ViewMessage.WIJZIGING.ToString()] != null)
            {
                ViewBag.Wijziging = "U heeft " + TempData[Enum.ViewMessage.WIJZIGING.ToString()] + " gewijzigt";
                TempData.Remove(Enum.ViewMessage.WIJZIGING.ToString());
            }

            // TempData Verwijdering
            if (TempData[Enum.ViewMessage.VERWIJDERING.ToString()] != null)
            {
                ViewBag.Wijziging = "U heeft " + TempData[Enum.ViewMessage.VERWIJDERING.ToString()] + " verwijdert";
                TempData.Remove(Enum.ViewMessage.VERWIJDERING.ToString());
            }

            return View(viewModel);
        }

        [CustomUnauthorized(Roles = "MANAGER")]
        public ActionResult Manager()
        {
            ManagerDbController controller = new ManagerDbController();
            
            ManagerViewModel model = new ManagerViewModel();
            List<ProductBase> bestSellerView = new List<ProductBase>();
            foreach(int id in controller.getBestSellers()){
                bestSellerView.Add(productDBController.GetProductByDetail(id));
            }
            List<ProductBase> worstSellerView = new List<ProductBase>();
            foreach (int id in controller.getWorsttSellers())
            {
                worstSellerView.Add(productDBController.GetProductByDetail(id));
            }
            model.bestSeller = bestSellerView;
            return View(model);
        }
    }
}
