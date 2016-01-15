using CBlokHerkansing.Models.Klant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.ViewModels.Klant
{
    public class KlantBaseEnAdresViewModel
    {
        public KlantBase klantBase { get; set; }
        public List<Adres> klantAdressen { get; set; }

        [Required(ErrorMessage = "Een verzend keuze is vereist")]
        [Range(1, int.MaxValue, ErrorMessage = "Een verzend keuze is vereist")]
        public int bestelKeuze { get; set; }

        [Required(ErrorMessage = "Een adres is vereist")]
        public int adresKeuze { get; set; }
    }
}