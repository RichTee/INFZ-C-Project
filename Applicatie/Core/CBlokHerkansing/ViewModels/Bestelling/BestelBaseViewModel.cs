using CBlokHerkansing.Models.Bestelling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.ViewModels.Bestelling
{
    public class BestelBaseViewModel
    {
        public BestellingBase bestelling { get; set; }

        public SelectList listStatus { get; set; }

        [Required(ErrorMessage = "U moet een bestel status selecteren")]
        public string SelectedStatus { get; set; }
    }
}