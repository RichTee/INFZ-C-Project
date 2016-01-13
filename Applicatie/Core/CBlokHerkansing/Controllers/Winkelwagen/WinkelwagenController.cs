using CBlokHerkansing.Controllers.Database;
using CBlokHerkansing.Models.Product;
using CBlokHerkansing.Models.Winkelwagen;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Winkelwagen
{
    public class WinkelwagenController : Controller
    {
        private ProductDBController productDBController = new ProductDBController();
        protected String CartKey = "MyCart";

        public ActionResult Winkelwagen()
        {
            if(Request.Cookies[CartKey] == null)
            {
                // Viewbag message telling the user to add an item
                return View();
            }

            // Get model from cookies
            WinkelwagenItem item = (WinkelwagenItem) JsonConvert.DeserializeObject<WinkelwagenItem>(Request.Cookies[CartKey].Value);

            return View(item);
        }

        // Add an item to the cart of the user
        public ActionResult AddToCart(int id)
        {
            if (Request.Cookies[CartKey] == null)
            {
                // Create cookie
                HttpCookie myCookie = new HttpCookie(CartKey);
                myCookie.Expires = DateTime.Now.AddDays(31d);
                Response.Cookies.Add(myCookie);
            }

            // Check if Cookie value exists
            // else continue
            if(Response.Cookies[CartKey].Value == null)
            {
                WinkelwagenItem wItem = new WinkelwagenItem();
                wItem.product = new List<ProductDetail>();
                wItem.hoeveelheid = new List<int>();
                Response.Cookies[CartKey].Value = JsonConvert.SerializeObject(wItem);
            }
            // Attach id to model by retrieving data from DB
            ProductDetail product = productDBController.GetProductAndDetail(id);

            // TODO: Check if Cookie value is WinkelwagenItem class
            
            // Check if item exists, if so, increment quantity.
            // else add item and quantity
            WinkelwagenItem item = JsonConvert.DeserializeObject<WinkelwagenItem>(Request.Cookies[CartKey].Value);

            if(CartItemExists(product, item))
            {
                // Get index value
                int index = item.product.FindIndex(p => p.product.ProductId == product.product.ProductId); // Can use method immediatly, instead of assigning variable
                item.hoeveelheid[index] += 1;

                // Modify response 
                Response.Cookies[CartKey].Value = JsonConvert.SerializeObject(item);
            }
            else
            {
                // Add Product
                item.product.Add(product);
                item.hoeveelheid.Add(1);
                
                // Modify Response
                Response.Cookies[CartKey].Value = JsonConvert.SerializeObject(item);
            }

            return RedirectToAction("Winkelwagen", "Winkelwagen");
        }

        // Check if item exists in cart
        private bool CartItemExists(ProductDetail product, WinkelwagenItem item)
        {
            if (item.product.Find(p => p.product.ProductId == product.product.ProductId) != null)
                return true;
            else
                return false;
        }

        public ActionResult WinkelwagenProductMinus(int id)
        {
            if (id == 0)
                return RedirectToAction("Winkelwagen", "Winkelwagen");

            WinkelwagenItem item = (WinkelwagenItem)JsonConvert.DeserializeObject<WinkelwagenItem>(Request.Cookies[CartKey].Value);
            ProductDetail product = productDBController.GetProductAndDetail(id);

            if (CartItemExists(product, item))
            {
                // Get index value
                int index = item.product.FindIndex(p => p.product.ProductId == product.product.ProductId); // Can use method immediatly, instead of assigning variable
                
                item.hoeveelheid[index] -= 1;
                if(item.hoeveelheid[index] <= 0)
                {
                    item.product.RemoveAt(index);
                    item.hoeveelheid.RemoveAt(index);
                }


                // Modify response 
                Response.Cookies[CartKey].Value = JsonConvert.SerializeObject(item);
            }

            return RedirectToAction("Winkelwagen", "Winkelwagen");
        }

        public ActionResult WinkelwagenProductPlus(int id)
        {
            if (id == 0)
                return RedirectToAction("Winkelwagen", "Winkelwagen");

            WinkelwagenItem item = (WinkelwagenItem)JsonConvert.DeserializeObject<WinkelwagenItem>(Request.Cookies[CartKey].Value);
            ProductDetail product = productDBController.GetProductAndDetail(id);

            if (CartItemExists(product, item))
            {
                // Get index value
                int index = item.product.FindIndex(p => p.product.ProductId == product.product.ProductId); // Can use method immediatly, instead of assigning variable

                if (item.hoeveelheid[index] >= product.voorraad)
                {
                    item.hoeveelheid[index] = product.voorraad;
                    return RedirectToAction("Winkelwagen", "Winkelwagen");
                }
                else
                {
                    item.hoeveelheid[index] += 1;
                }

                // Modify response 
                Response.Cookies[CartKey].Value = JsonConvert.SerializeObject(item);
            }

            return RedirectToAction("Winkelwagen", "Winkelwagen");
        }

        public ActionResult WinkelwagenVerwijderProduct(int id)
        {
            if (id == 0)
                return RedirectToAction("Winkelwagen", "Winkelwagen");

            WinkelwagenItem item = (WinkelwagenItem)JsonConvert.DeserializeObject<WinkelwagenItem>(Request.Cookies[CartKey].Value);
            ProductDetail product = productDBController.GetProductAndDetail(id);

            if(CartItemExists(product, item))
            {
                // Get index value
                int index = item.product.FindIndex(p => p.product.ProductId == product.product.ProductId); // Can use method immediatly, instead of assigning variable
                item.product.RemoveAt(index);
                item.hoeveelheid.RemoveAt(index);

                // Modify response 
                Response.Cookies[CartKey].Value = JsonConvert.SerializeObject(item);
            }

            return RedirectToAction("Winkelwagen", "Winkelwagen");
        }
    }
}