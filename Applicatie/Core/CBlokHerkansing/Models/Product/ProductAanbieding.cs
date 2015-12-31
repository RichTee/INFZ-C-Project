using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Product
{
    public class ProductAanbieding
    {
        public int AanbiedingId { get; set; }
        public string BeginDatum { get; set; }
        public string EindDatum { get; set; }
        public int KortingsPercentage { get; set; }
        public double KortingsBedrag { get; set; }
    }
}