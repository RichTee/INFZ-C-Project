using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naam is vereist")]
        [MaxLength(45, ErrorMessage="Een naam mag maximaal 45 tekens bevatten")]
        public string Naam { get; set; }
        [Required(ErrorMessage="Omschrijving is vereist")]
        [MaxLength(100, ErrorMessage="Een omschrijving mag maximaal 100 tekens bevatten")]
        public string Omschrijving { get; set; }
        public int HoofdcategorieId { get; set; }
    }
}