using CBlokHerkansing.Authorisation;
using CBlokHerkansing.Models.Bestelling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Bestelling
{
    public class BestellingController : Controller
    {
        // GET: Bestelling
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
    }
}