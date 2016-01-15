using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBlokHerkansing.Controllers.Database;
using CBlokHerkansing.Models.Product;
using CBlokHerkansing.ViewModels.Product;

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

        public ActionResult Search(string search, int type)
        {
            //product search from product name
            ProductDBController productController = new ProductDBController();
            if (type == 0)
            {                
                return View(productController.getListProductFromNameSearch(search));
            }
            else
            {
                //product search from categorie id
                CategorieDbController categorie = new CategorieDbController();
                return View(productController.getListProductFromCategorie(categorie.getCategorieIdByName(search)));
            }
        }
    }
}