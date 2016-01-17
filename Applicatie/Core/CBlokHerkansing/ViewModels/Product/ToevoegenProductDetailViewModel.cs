using CBlokHerkansing.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.ViewModels.Product
{
    public class ToevoegenProductDetailViewModel
    {
        public List<ProductBase> productBaseList { get; set; }

        public ProductDetail productDetail { get; set; }

        public SelectList listStatus { get; set; }

        [Required(ErrorMessage = "U moet een bestel status selecteren")]
        public string SelectedStatus { get; set; }
    }
}