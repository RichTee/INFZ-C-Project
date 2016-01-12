using CBlokHerkansing.Models.Bestelling;
using CBlokHerkansing.Models.Klant;
using CBlokHerkansing.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.ViewModels.Account
{
    public class BeheerderViewModel
    {
        public List<KlantBase> klantOverzicht { get; set; }
        public List<ProductBase> productBaseOverzicht { get; set; } // TODO: Remove, detailoverzicht heeft alle informatie van base al
        public List<ProductDetail> productDetailOverzicht { get; set; }
        public List<ProductAanbieding> productAanbiedingOverzicht { get; set; }
        public List<BestelRegel> bestellingDetailOverzicht { get; set; }
    }
}