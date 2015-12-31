using CBlokHerkansing.Models.Account;
using CBlokHerkansing.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.ViewModels.Account
{
    public class BeheerderViewModel
    {
        public List<Klant> klantOverzicht { get; set; }
        public List<ProductBase> productBaseOverzicht { get; set; }
        public List<ProductAanbieding> productAanbiedingOverzicht { get; set; }
    }
}