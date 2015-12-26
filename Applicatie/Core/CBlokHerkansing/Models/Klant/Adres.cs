using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Klant
{
    public class Adres
    {
        public int id { get; set; }
        public string straat { get; set; }
        public string postcode { get; set; }
        public string huisnummer { get; set; }
        public string huisnummertoevoeging { get; set; }
        public string stad { get; set; }
        public int gebruikerId { get; set; }

    }
}