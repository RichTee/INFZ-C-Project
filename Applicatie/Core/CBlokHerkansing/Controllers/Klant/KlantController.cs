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
                    if (!accountDBController.checkGebruikerDuplicaat(User.Identity.Name))
                        return View(klant);

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
                klantDBController.VerwijderKlant(email);
                TempData[Enum.ViewMessage.VERWIJDERING.ToString()] = "het";
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
            }
            return RedirectToAction("Beheer", "Account");
        }
    }
}