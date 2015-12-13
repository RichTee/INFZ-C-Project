using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Account
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Een Gebruikersnaam is verplicht")] // Dit MOET aanwezig zijn
        [StringLength(25, ErrorMessage = "Een Gebruikersnaam mag maximaal 25 karakters hebben")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Een Wachtwoord is verplicht")]
        [StringLength(25, ErrorMessage = "Een Wachtwoord mag maximaal 25 karakters hebben")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}