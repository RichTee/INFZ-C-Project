using CBlokHerkansing.Authorisation;
using CBlokHerkansing.Controllers.Database;
using CBlokHerkansing.Models.Account;
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
        
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult ToevoegenKlant()
        {
            return View();
        }

        [HttpPost]
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult ToevoegenKlant(Klant klant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    klantDBController.InsertKlant(klant);
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

        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult WijzigKlant(string email)
        {
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
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult WijzigKlant(Klant klant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    klantDBController.UpdateKlant(klant);
                    return RedirectToAction("index", "Home"); // TODO: Redirect naar user profiel of beheerder
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
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
            }
            return RedirectToAction("Beheer", "Account");
        }
    }
}