using CBlokHerkansing.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.ViewModels.Product
{
    public class ProductItemViewModel
    {
        public ProductBase productBaseOverzicht { get; set; }
        public List<ProductDetail> productDetailOverzicht { get; set; }
    }
}