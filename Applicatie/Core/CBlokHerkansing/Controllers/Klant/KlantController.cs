using CBlokHerkansing.Authorisation;
using CBlokHerkansing.Controllers.Database;
using CBlokHerkansing.Models.Klant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.User
{
    public class KlantController : Controller
    {
        private KlantDBController klantDBController = new KlantDBController();
        private AccountDBController accountDBController = new AccountDBController();
        private BestellingDBController bestellingDBController = new BestellingDBController();

        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult ToevoegenKlant()
        {
            return View();
        }

        [HttpPost]
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult ToevoegenKlant(KlantBase klant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (accountDBController.checkGebruikerDuplicaat(klant.Email))
                    {
                        ViewBag.Foutmelding = "Email bestaat al";
                        return View(klant);
                    }

                    klantDBController.InsertKlant(klant);

                    TempData[Enum.ViewMessage.TOEVOEGING.ToString()] = "Klant Id: " + klant.Id + ", Email: " + klant.Email;

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
                return View(klant);
            }
        }

        [CustomUnauthorized(Roles = "KLANT, ADMIN")]
        public ActionResult WijzigKlant(string email)
        {
            try
            {
                KlantBase klant = klantDBController.GetKlantInformatie(email);
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
        public ActionResult WijzigKlant(KlantBase klant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    klantDBController.UpdateKlant(klant);

                    if (User.IsInRole("KLANT"))
                    {
                        TempData[Enum.ViewMessage.WIJZIGING.ToString()] = "uw klant gegevens";

                        return RedirectToAction("Profiel", "Account");
                    }
                    else if (User.IsInRole("ADMIN"))
                    {
                        TempData[Enum.ViewMessage.WIJZIGING.ToString()] = klant.Email;

                        return RedirectToAction("Beheer", "Account");
                    }
                    return RedirectToAction("index", "Home");
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

        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult VerwijderKlant(string email)
        {
            try
            {
                int id = klantDBController.GetKlantId(email);
                if (bestellingDBController.CheckActiveBestellingenByAdres(klantDBController.GetAdresId(id)))
                {
                    TempData[Enum.ViewMessage.FOUTMELDING.ToString()] = "Klant heeft een actieve bestelling!";
                    return RedirectToAction("Beheer", "Account");
                }

                klantDBController.VerwijderKlant(email);
                TempData[Enum.ViewMessage.VERWIJDERING.ToString()] = "het";
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
            }
            return RedirectToAction("Beheer", "Account");
        }

        /*
         * 
         * Adres
         * 
         */

        [CustomUnauthorized(Roles = "KLANT, ADMIN")]
        public ActionResult ToevoegenAdres()
        {
            return View();
        }

        [HttpPost]
        [CustomUnauthorized(Roles = "KLANT, ADMIN")]
        public ActionResult ToevoegenAdres(Adres adres)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    adres.GebruikerId = klantDBController.GetKlantId(User.Identity.Name);
                    klantDBController.InsertAdres(adres);
                    TempData[Enum.ViewMessage.TOEVOEGING.ToString()] = "Straat: " + adres.Straat + ", Postcode: " + adres.Postcode;

                    if (User.IsInRole("KLANT"))
                        return RedirectToAction("Profiel", "Account");

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
                return View(adres);
            }
        }

        [CustomUnauthorized(Roles = "KLANT, ADMIN")]
        public ActionResult WijzigAdres(int id)
        {
            try
            {
                Adres adres = klantDBController.GetKlantAdresByAdresId(id);
                return View(adres);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }
        }

        [HttpPost]
        [CustomUnauthorized(Roles = "KLANT, ADMIN")]
        public ActionResult WijzigAdres(Adres adres)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    klantDBController.UpdateAdres(adres);

                    if (User.IsInRole("KLANT"))
                    {
                        TempData[Enum.ViewMessage.WIJZIGING.ToString()] = "uw adres gegevens zijn";

                        return RedirectToAction("Profiel", "Account");
                    }
                    else if (User.IsInRole("ADMIN"))
                    {
                        TempData[Enum.ViewMessage.WIJZIGING.ToString()] = adres.Straat + ", " + adres.Huisnummer;

                        return RedirectToAction("Beheer", "Account");
                    }
                    return RedirectToAction("index", "Home");
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                    return View();
                }
            }
            else
            {
                return View(adres);
            }
        }

        [CustomUnauthorized(Roles = "KLANT, ADMIN")]
        public ActionResult VerwijderAdres(int id)
        {
            try
            {
                // TODO: Check if User is klant, if so, only give access to own data

                if(bestellingDBController.CheckActiveBestellingenByAdres(id))
                {
                    TempData[Enum.ViewMessage.FOUTMELDING.ToString()] = "U heeft een actieve bestelling op dit adres!" + "\n Adres kan pas verwijdert worden wanneer de bestelling bij u ontvangen is";
                    return RedirectToAction("Profiel", "Account");
                }

                klantDBController.VerwijderAdres(id);
                TempData[Enum.ViewMessage.VERWIJDERING.ToString()] = "het";
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
            }
            if (User.IsInRole("KLANT"))
                return RedirectToAction("Profiel", "Account");
            return RedirectToAction("Beheer", "Account");
        }
    }
}