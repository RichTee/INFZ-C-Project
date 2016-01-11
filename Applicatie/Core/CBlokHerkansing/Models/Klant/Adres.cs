using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Klant
{
    public class Adres
    {
        public int Id { get; set; }
        public string Straat { get; set; }
        public string Postcode { get; set; }
        public int Huisnummer { get; set; }
        public string HuisnummerToevoegsel { get; set; }
        public string Stad { get; set; }
        public int GebruikerId { get; set; }

    }
}