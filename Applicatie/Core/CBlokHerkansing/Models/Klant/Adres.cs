using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Klant
{
    public class Adres
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Een straat is verplicht")]
        [StringLength(30, ErrorMessage = "Een straat mag maximaal 30 karakters hebben")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Vul een valide straat in")]
        public string Straat { get; set; }

        [Required(ErrorMessage = "Een postcode is verplicht")]
        [RegularExpression(@"^[1-9][\d]{3}\s?(?!(sa|sd|ss|SA|SD|SS))([a-eghj-npr-tv-xzA-EGHJ-NPR-TV-XZ]{2})$", ErrorMessage = "Vul een valide postcode in")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Een huisnummer is verplicht")]
        [Range(0, 25000, ErrorMessage = "Vul een valide huisnummer in ")]
        [DataAnnotationsExtensions.Integer(ErrorMessage = "Vul een valide huisnummer in.")]
        public int Huisnummer { get; set; }

        // TODO: REGEX
        public string HuisnummerToevoegsel { get; set; }

        [Required(ErrorMessage = "Een Stad is verplicht")]
        [StringLength(30, ErrorMessage = "Een Stad mag maximaal 30 karakters hebben")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Vul een valide stad in")]
        public string Stad { get; set; }

        public int GebruikerId { get; set; }

    }
}