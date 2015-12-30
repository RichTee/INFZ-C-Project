using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Product
{
    public class ProductBase
    {
        public int ProductId { get; set; }
        public string Naam { get; set; }
        public string Omschrijving { get; set; }
        public int CategorieId { get; set; }
    }
}