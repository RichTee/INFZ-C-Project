using CBlokHerkansing.Controllers.Database;
using CBlokHerkansing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Beheer
{
    public class CategorieController : Controller
    {
        public ActionResult AlleCategorieen()
        {
            CategorieDbController controller = new CategorieDbController();
            return View(controller.getListWithAllCategorieen());
        }

        public ActionResult VoegCategorieToe()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VoegCategorieToe(Categorie categorie)
        {
            if(ModelState.IsValid){
            CategorieDbController controller = new CategorieDbController();
            controller.insertCategorie(categorie);
            ViewBag.succes = "Het toevoegen van het product is gelukt";
            }
            
            return View();
        }
        [HttpGet]
        public ActionResult UpdateCategorie(int categorieId)
        {
            CategorieDbController controller = new CategorieDbController();
            return View(controller.getCategorieFromId(categorieId));
        }
        [HttpPost]
        public ActionResult UpdateCategorie(Categorie categorie)
        {
            CategorieDbController controller = new CategorieDbController();
            controller.UpdateCategorie(categorie);
            ViewBag.Succes = "Het updaten van deze categorie is gelukt";
            return View(categorie);
        }
        [HttpGet]
        public ActionResult DeleteCategorie(int categorieId)
        {
            CategorieDbController controller = new CategorieDbController();
            controller.DeleteCategorie(categorieId);
            return RedirectToAction("AlleCategorieen", "Categorie");
        }
    }
}