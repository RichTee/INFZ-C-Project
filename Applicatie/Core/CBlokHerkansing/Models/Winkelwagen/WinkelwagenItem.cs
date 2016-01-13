using CBlokHerkansing.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Winkelwagen
{
    public class WinkelwagenItem
    {
        public List<ProductDetail> product { get; set; }
        public List<Int32> hoeveelheid { get; set; }
    }
}