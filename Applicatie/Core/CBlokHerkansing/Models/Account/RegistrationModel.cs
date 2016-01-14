//using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CBlokHerkansing.Models.Account
{
    public class RegistrationModel
    {
        /*
         * TODO:
         * Check String Length does not exceed Database VarChar length 
         * Encrypt Password with BCrypt
         */
        [CustomEmailValidator] // Is al required
        [StringLength(25, ErrorMessage = "Een Email mag maximaal 25 karakters hebben")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Een Wachtwoord is verplicht")]
        [StringLength(25, ErrorMessage = "Een Wachtwoord mag maximaal 25 karakters hebben")]
        [DataType(DataType.Password)]
        public string Wachtwoord { get; set; }

        [Required(ErrorMessage = "Een Wachtwoord is verplicht")] // Logisch gezien is Required niet nodig indien je een CompareAttribute hebt.
        [CompareAttribute("Wachtwoord", ErrorMessage = "Wachtwoorden komen niet overeen.")]
        [DataType(DataType.Password)]
        public string WachtwoordCheck { get; set; }
        [Required(ErrorMessage = "Een voornaam is verplicht")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Vul een valide Voornaam in")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Een achternaam is verplicht")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Vul een valide Achternaam in")]
        public string Achternaam { get; set; }

        [Required(ErrorMessage = "Een telefoonnummer is verplicht. Voorbeeld: 0640535921")]
        // TODO: REGEX telefoonnummer
        [DataAnnotationsExtensions.Integer(ErrorMessage = "Mogen alleen nummers zijn! Voorbeeld: 0640535921")]
        public int Telefoonnummer { get; set; }
    }
}