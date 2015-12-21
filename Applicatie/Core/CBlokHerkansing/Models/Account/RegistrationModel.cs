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
         * Set up regeular expression for email.
         * Encrypt Password with BCrypt
         */
        [Required(ErrorMessage = "Een email is verplicht")]
        //[RegularExpression(reg ,ErrorMessage = "Dit is geen geldig email adres.")]
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
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Een achternaam is verplicht")]
        public string Achternaam { get; set; }

        [Required(ErrorMessage = "Een telefoonnummer is verplicht")]
        public int Telefoonnummer { get; set; }
    }
}