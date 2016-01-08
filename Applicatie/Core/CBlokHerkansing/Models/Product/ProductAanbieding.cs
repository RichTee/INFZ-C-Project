using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Product
{
    public class ProductAanbieding
    {
        public int AanbiedingId { get; set; }

        [Required(ErrorMessage = "Begin Datum is een verplicht veld")]
        public string BeginDatum { get; set; }

        [Required(ErrorMessage = "Eind Datum is een verplicht veld")]
        public string EindDatum { get; set; }

        [Required(ErrorMessage = "Kortingspercentage is een verplicht veld")]
        public int KortingsPercentage { get; set; }
    }
}