using CBlokHerkansing.Authorisation;
using CBlokHerkansing.Controllers.Database;
using CBlokHerkansing.Models.Bestelling;
using CBlokHerkansing.Models.Klant;
using CBlokHerkansing.Models.Winkelwagen;
using CBlokHerkansing.ViewModels.Bestelling;
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
            BestelBaseViewModel bestelling = new BestelBaseViewModel();
            bestelling.bestelling = bestellingDBController.GetBestellingById(id);
            bestelling.listStatus = selectListBestelling();

            return View(bestelling);
        }

        [HttpPost]
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult WijzigBestelling(BestelBaseViewModel bestellingBaseModel)
        {
            bestellingBaseModel.bestelling.BezorgStatus = bestellingBaseModel.SelectedStatus;
            if (ModelState.IsValid)
            {
                try
                {
                    bestellingDBController.UpdateBestellingStatus(bestellingBaseModel.bestelling.BestellingId, bestellingBaseModel.bestelling.BezorgStatus);
                    TempData[Enum.ViewMessage.WIJZIGING.ToString()] = " Bestelling Id: " + bestellingBaseModel.bestelling.BestellingId;

                    return RedirectToAction("Beheer", "Account");
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                    return View();
                }
            }
            else
            {
                bestellingBaseModel.listStatus = selectListBestelling();
                return View(bestellingBaseModel);
            }
        }

        private SelectList selectListBestelling()
        {
            List<String> list = new List<String>();
            list.Add(Enum.BestelStatus.PENDING.ToString());
            list.Add(Enum.BestelStatus.PROCESSING.ToString());
            list.Add(Enum.BestelStatus.DELIVERED.ToString());

            var selectListItems = list.Select(x => new SelectListItem
            {
                Text = x.ToString(),
                Value = x.ToString()
            }).ToList();

            return new SelectList(selectListItems, "Text", "Value");
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