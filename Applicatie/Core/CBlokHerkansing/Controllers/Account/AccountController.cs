using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Login", "AccountController"); // Ternary Operator when already logged in to redirect to somewhere else.
        }

        public ActionResult Login()
        {
            return View();
        }
        
        public ActionResult Logout()
        {
            return RedirectToAction("Index", "HomeController");
        }
    }
}