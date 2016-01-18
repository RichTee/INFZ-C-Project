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

        /*
         * 
         * Product
         * 
         */
        public SelectList listProduct { get; set; }

        [Required(ErrorMessage = "U moet een product selecteren")]
        public int SelectedProduct { get; set; }


        /*
         * 
         * Maat
         * 
         */
        public SelectList listMaat { get; set; }

        [Required(ErrorMessage = "U moet een maat selecteren")]
        public int SelectedMaat { get; set; }
    }
}