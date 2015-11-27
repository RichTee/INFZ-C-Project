using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Brownie candy canes halvah marshmallow halvah chocolate. Bonbon oat cake cake fruitcake chocolate cake. Lemon drops candy canes chocolate cake gummies tiramisu cheesecake jelly-o jujubes sweet. Icing toffee brownie gummi bears halvah.";

            return View();
        }
    }
}