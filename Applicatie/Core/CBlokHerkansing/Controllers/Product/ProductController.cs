using CBlokHerkansing.Authorisation;
using CBlokHerkansing.Controllers.Database;
using CBlokHerkansing.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Product
{
    public class ProductController : Controller
    {
        private ProductDBController productDBController = new ProductDBController();

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult WijzigProduct(int id)
        {
            try
            {
                ProductViewModel product = productDBController.GetProductAndDetail(id);
                return View(product);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }
        }

        [HttpPost]
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult WijzigProduct(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    productDBController.UpdateProductAndDetail(productViewModel);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                    return View();
                }
            }
            else
            {
                return View(productViewModel);
            }
        }
    }
}