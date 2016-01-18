using CBlokHerkansing.Authorisation;
using CBlokHerkansing.Controllers.Database;
using CBlokHerkansing.Models.Product;
using CBlokHerkansing.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Product
{
    public class ProductController : Controller
    {
        private ProductDBController productDBController = new ProductDBController();
        private AanbiedingDBController aanbiedingDBController = new AanbiedingDBController();

        // GET: Product
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult ToevoegenProductAanbieding()
        {
            return View();
        }
        
        [HttpPost]
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult ToevoegenProductAanbieding(ProductAanbieding aanbieding)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    aanbiedingDBController.InsertAanbieding(aanbieding);
                    TempData[Enum.ViewMessage.TOEVOEGING.ToString()] = "Aanbieding Id: " + aanbieding.AanbiedingId + ", KortingsPercentage: " + aanbieding.KortingsPercentage;

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
                return View(aanbieding);
            }
        }

        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult WijzigProductAanbieding(int id /* TODO: Minimaliseer queries naar DB door model door te geven, en niet ID op te zoeken naar de DB */)
        {
            try
            {
                ProductAanbieding aanbieding = aanbiedingDBController.GetAanbieding(id);
                return View(aanbieding);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View(); /* TODO: Error redirect pagina ipv lege view returnen */
            }
        }

        [HttpPost]
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult WijzigProductAanbieding(ProductAanbieding aanbieding)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    aanbiedingDBController.WijzigAanbieding(aanbieding);
                    TempData[Enum.ViewMessage.WIJZIGING.ToString()] = "Aanbieding Id: " + aanbieding.AanbiedingId;
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
                return View(aanbieding);
            }
        }

        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult VerwijderProductAanbieding(int id)
        {
            try
            {
                aanbiedingDBController.VerwijderAanbieding(id);
                TempData[Enum.ViewMessage.VERWIJDERING.ToString()] = "de aanbieding";
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
            }
            return RedirectToAction("Beheer", "Account");
        }

        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult ToevoegenProduct()
        {
            return View();
        }

        [HttpPost]
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult ToevoegenProduct(ProductBase product, Picture picture)
        {
            if (ModelState.IsValid )
            {
                List<string> extensie = new List<string>(){
                ".jpg",
                ".JPG",
                ".png",
                ".PNG",
                ".jpeg",
                ".JPEG"
            };
                try
                {
                    if (picture.File.ContentLength > 0 && extensie.Contains(Path.GetExtension(picture.File.FileName)))
                        {

                            var fileName = Path.GetFileName(picture.File.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/ProductImages"), fileName);
                            picture.File.SaveAs(path);
                            product.AfbeeldingPath = "../../Content/ProductImages/" + fileName;
                        }
                   int id = productDBController.insertProductAndAfbeeldingForToeveogenProductDetail(product);

                    TempData[Enum.ViewMessage.TOEVOEGING.ToString()] = "Product Id: " + product.ProductId + ", Naam: " + product.Naam;
                    return RedirectToAction("ToevoegenProductDetail", "Product", new { productId = id });
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                    return View();
                }
            }
            else
            {
                return View(product);
            }
        }

        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult WijzigProduct(int id)
        {
            try
            {
                ProductDetail product = productDBController.GetProductAndDetail(id);
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
        public ActionResult WijzigProduct(ProductDetail productDetailModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    productDBController.UpdateProductAndDetail(productDetailModel);
                    TempData[Enum.ViewMessage.WIJZIGING.ToString()] = "Product: " + productDetailModel.product.Naam;
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
                return View(productDetailModel);
            }
        }


        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult VerwijderProductEnDetail(int id)
        {
            try
            {
                TempData[Enum.ViewMessage.VERWIJDERING.ToString()] = "het";
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
            }
            return RedirectToAction("Beheer", "Account");
        }

        /*
         * 
         * ProductDetail
         * 
         */
        
        [CustomUnauthorized(Roles = "ADMIN")]
        [HttpGet]
        public ActionResult ToevoegenProductDetail(int id)
        {
            return View(id);
        }

        [HttpPost]
        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult ToevoegenProductDetail(ProductDetail productDetail, int anotherOne)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    productDBController.InsertProductDetail(productDetail);
                    TempData[Enum.ViewMessage.TOEVOEGING.ToString()] = "Detail Id: " + productDetail.detailId + ", Voorraad: " + productDetail.voorraad + ", Maat: " + productDetail.maat + ", Verkoopprijs: " + productDetail.verkoopprijs + ", Inkoopprijs: " + productDetail.inkoopprijs;
                    if (anotherOne == 1)
                    {
                        return RedirectToAction("ToevoegenProductDetail", "Product", new { productId = productDetail.productid });
                    }
                    else
                    {
                        return RedirectToAction("Beheer", "Account");
                    }
                    
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                    return View();
                }
            }
            else
            {
                return View(productDetail);
            }
        }

        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult WijzigProductDetail(int id)
        {
            try
            {
                ProductDetail product = productDBController.GetProductDetail(id);
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
        public ActionResult WijzigProductDetail(ProductDetail productDetailModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    productDBController.UpdateProductDetail(productDetailModel);
                    TempData[Enum.ViewMessage.WIJZIGING.ToString()] = "Detail: " + productDBController.GetProductByDetail(productDetailModel.detailId).Naam;

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
                return View(productDetailModel);
            }
        }


        [CustomUnauthorized(Roles = "ADMIN")]
        public ActionResult VerwijderProductDetail(int id)
        {
            try
            {
                TempData[Enum.ViewMessage.VERWIJDERING.ToString()] = "het detail";
                productDBController.VerwijderProductDetail(id);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
            }

            return RedirectToAction("Beheer", "Account");
        }
    }
}