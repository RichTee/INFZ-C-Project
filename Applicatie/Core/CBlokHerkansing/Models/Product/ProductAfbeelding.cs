using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Product
{
    public class ProductAfbeelding
    {
        public int AfbeeldingId { get; set; }
        public string Omschrijving { get; set; }
        public string Locatie { get; set; }
        public string Titel { get; set; }
        public int ProductId { get; set; }
    }
}