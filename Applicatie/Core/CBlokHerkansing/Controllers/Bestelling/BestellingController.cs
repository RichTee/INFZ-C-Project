using CBlokHerkansing.Authorisation;
using CBlokHerkansing.Controllers.Database;
using CBlokHerkansing.Models.Bestelling;
using CBlokHerkansing.Models.Klant;
using CBlokHerkansing.Models.Winkelwagen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Bestelling
{
    public class BestellingController : Controller
    {
        private KlantDBController klantDBController = new KlantDBController();
        private BestellingDBController bestellingDBController = new BestellingDBController();
        protected String CartKey = "MyCart";

        // GET: Bestelling
        [HttpPost]
        public ActionResult Bestellen(WinkelwagenItem winkelwagenItemModel)
        {
            /*if (winkelwagenItemModel.product == null || winkelwagenItemModel.hoeveelheid == null)
                return RedirectToAction("Index", "Home");
            */

            // TODO: Check if Cookie value is WinkelwagenItem class

            // Check if item exists, if so, increment quantity.
            // else add item and quantity
            WinkelwagenItem item = JsonConvert.DeserializeObject<WinkelwagenItem>(Request.Cookies[CartKey].Value);
            if (item.product == null)
                return RedirectToAction("Index", "Home");
            // Temp
            bestellingAfronden(item);

            return View();
        }

        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult WijzigBestelling(int id)
        {
            return View();
        }

        [HttpPost]
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult WijzigBestelling(BestellingBase bestellingBaseModel)
        {
            return View();
        }

        private bool bestellingAfronden(WinkelwagenItem winkelwagenItemModel)
        {
            // Retrieve User Data
            KlantBase klant = klantDBController.GetKlantInformatie(User.Identity.Name);
            int adresId = klantDBController.GetAdresId(klant.Id);
            if (adresId == 0)
                RedirectToAction("Winkelwagen", "Winkelwagen"); // Temp Data over Adres niet ingevult

            int aanbiedingId = 0; // Get AanbiedingId indien beschikbaar

            // Insert Bestelling
            if(bestellingDBController.InsertBestelling(winkelwagenItemModel, "2016-01-28" /*TODO: Datum */, klant.Id, adresId, aanbiedingId))
            {
                // Succesful Insert
                if (Request.Cookies[CartKey] != null)
                {
                    // Empty Winkelwagen
                    Response.Cookies[CartKey].Expires = DateTime.Now.AddDays(-1);
                }
                RedirectToAction("Profiel", "Account"); // Temp data meegeven
            }
            else
            {
                // Unsuccesful Insert
                RedirectToAction("Winkelwagen", "Winkelwagen"); // Temp Data meegeven
            }

            return true;
        }
    }
}