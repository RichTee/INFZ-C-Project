using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBlokHerkansing.Controllers.Database;
using CBlokHerkansing.Models.Product;

namespace CBlokHerkansing.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ProductDBController productController = new ProductDBController();
            return View(productController.GetProducten());
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}