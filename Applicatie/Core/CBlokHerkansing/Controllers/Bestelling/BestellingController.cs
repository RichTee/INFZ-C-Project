using CBlokHerkansing.Authorisation;
using CBlokHerkansing.Controllers.Database;
using CBlokHerkansing.Models.Bestelling;
using CBlokHerkansing.Models.Klant;
using CBlokHerkansing.Models.Winkelwagen;
using CBlokHerkansing.ViewModels.Bestelling;
using CBlokHerkansing.ViewModels.Klant;
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
        [CustomUnauthorized(Roles = "KLANT")]
        public ActionResult Bestellen()
        {
            KlantBase klantBase = klantDBController.GetKlantInformatie(User.Identity.Name);
            List<Adres> klantAdres = klantDBController.GetKlantAdressen(klantDBController.GetKlantId(User.Identity.Name));
            KlantBaseEnAdresViewModel viewModel = new KlantBaseEnAdresViewModel();
            viewModel.klantBase = klantBase;
            viewModel.klantAdressen = klantAdres;

            // TempData Foutmelding
            if (TempData[Enum.ViewMessage.FOUTMELDING.ToString()] != null)
            {
                ViewBag.Foutmelding = TempData[Enum.ViewMessage.FOUTMELDING.ToString()];
                TempData.Remove(Enum.ViewMessage.FOUTMELDING.ToString());
            }

            return View(viewModel);
        }

        [HttpPost]
        [CustomUnauthorized(Roles = "KLANT")]
        public ActionResult Bestellen(KlantBaseEnAdresViewModel viewModel)
        {
                try
                {
                    if (viewModel.adresKeuze == 0)
                    {
                        TempData[Enum.ViewMessage.FOUTMELDING.ToString()] = "U moet een adres selecteren!";
                        return RedirectToAction("Bestellen");
                    }

                    WinkelwagenItem item = JsonConvert.DeserializeObject<WinkelwagenItem>(Request.Cookies[CartKey].Value);
                    if (item.product == null)
                        return RedirectToAction("Index", "Home");

                    string bestelKeuze = "";
                    int adresKeuze = viewModel.adresKeuze;
                    switch (viewModel.bestelKeuze)
                    {
                        case 0:
                            bestelKeuze = Enum.VerzendKeuze.ONLINE.ToString();
                            break;
                        case 1:
                            bestelKeuze = Enum.VerzendKeuze.FACTUUR.ToString();
                            break;
                        default:
                            break;
                    }

                    bestellingAfronden(item, bestelKeuze, adresKeuze);
                    int klantId = klantDBController.GetKlantId(User.Identity.Name);
                    if (klantDBController.CheckGebruikerGoldMember(klantId))
                    {
                        klantDBController.UpdateGoldMember(klantId);
                    }
                    TempData[Enum.ViewMessage.TOEVOEGING.ToString()] = "uw bestelling naar ons verzonden. Deze is in ons process";

                    return RedirectToAction("Profiel", "Account");
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                    return View();
                }
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

        private bool bestellingAfronden(WinkelwagenItem winkelwagenItemModel, string bestelKeuze, int adres)
        {
            // Retrieve User Data
            KlantBase klant = klantDBController.GetKlantInformatie(User.Identity.Name);
            int adresId = klantDBController.GetAdresId(klant.Id);
            if (adresId == 0)
                RedirectToAction("Winkelwagen", "Winkelwagen"); // Temp Data over Adres niet ingevult

            int aanbiedingId = 0; // TODO: Get aanbieding

            // Gold member = 10% korting
            if (klantDBController.CheckGebruikerGoldMember(klant.Id))
            {
                for(int i = 0; i < winkelwagenItemModel.product.Count; i++)
                {
                    // 10% korting
                    winkelwagenItemModel.product[i].verkoopprijs = winkelwagenItemModel.product[i].verkoopprijs * 0.9;
                }
            }

            // Insert Bestelling
            if(bestellingDBController.InsertBestelling(winkelwagenItemModel, DateTime.Today.ToString("yyyy/M/%d"), klant.Id, adres, bestelKeuze, aanbiedingId))
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