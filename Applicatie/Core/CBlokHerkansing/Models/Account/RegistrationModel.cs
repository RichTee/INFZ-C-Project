using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        [Required(ErrorMessage = "Een Gebruikersnaam is verplicht")] // Dit MOET aanwezig zijn
        [StringLength(25, ErrorMessage = "Een Gebruikersnaam mag maximaal 25 karakters hebben")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Een Wachtwoord is verplicht")]
        [StringLength(25, ErrorMessage = "Een Wachtwoord mag maximaal 25 karakters hebben")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Een Wachtwoord is verplicht")]
        [EqualTo("Password", ErrorMessage = "Wachtwoorden komen niet overeen.")]
        [DataType(DataType.Password)]
        public string CheckPassword { get; set; }
        [Required(ErrorMessage = "Een voornaam is verplicht")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Een achternaam is verplicht")]
        public string Achternaam { get; set; }

        [Required(ErrorMessage = "Een telefoonnummer is verplicht")]
        public int Telefoonnummer { get; set; }

        [Required(ErrorMessage = "Een email is verplicht")]
        [RegularExpression("[A-Z0-9._%+-]+@[A-Z0-9.-]+.[A-Z]{2,})",ErrorMessage = "Dit is geen gledig email adres.")]
        public string Email { get; set; }
    }
}