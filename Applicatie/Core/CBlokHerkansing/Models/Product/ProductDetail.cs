using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Product
{
    public class ProductDetail
    {
        public int detailId { get; set; }
        public double verkoopprijs { get; set; }
        public double inkoopprijs { get; set; }
        public int maat { get; set; } // FK?
        public string kleur { get; set; } // FK?
        public int voorraad { get; set; }
        public int productId { get; set; }
    }
}