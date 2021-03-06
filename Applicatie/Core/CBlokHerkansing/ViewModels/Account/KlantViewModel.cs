﻿using CBlokHerkansing.Models.Bestelling;
using CBlokHerkansing.Models.Klant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.ViewModels.Account
{
    public class KlantViewModel
    {
        public KlantBase klantOverzicht { get; set; }

        public List<Adres> adresOverzicht { get; set; }

        public List<BestelRegel> bestellingOverzicht { get; set; }
    }
}