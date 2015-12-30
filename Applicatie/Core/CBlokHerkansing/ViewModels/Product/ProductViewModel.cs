using CBlokHerkansing.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.ViewModels.Product
{
    public class ProductViewModel
    {
        public ProductBase productBaseOverzicht { get; set; }
        public ProductDetail productDetailOverzicht { get; set; }
    }
}