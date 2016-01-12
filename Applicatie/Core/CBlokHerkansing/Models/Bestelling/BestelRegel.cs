using CBlokHerkansing.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Bestelling
{
    public class BestelRegel
    {
        public int RegelId { get; set; }
        public int Hoeveelheid { get; set; }
        public string Datum { get; set; }
        public BestellingBase Bestelling { get; set; }
        public ProductDetail ProductDetail { get; set; }
        public ProductAanbieding ProductAanbieding { get; set; }
    }
}