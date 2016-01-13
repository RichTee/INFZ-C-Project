using CBlokHerkansing.Models.Klant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Bestelling
{
    public class BestellingBase
    {
        public int BestellingId { get; set; }
        public string BezorgStatus { get; set; }
        public string BezorgTijd { get; set; }
        public string BestelDatum { get; set; }
        public Adres Adres { get; set; }
        public KlantBase Gebruiker { get; set; }
    }
}