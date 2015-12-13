using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Account
{
    public class LoginModel
    {
        /*
         * TODO:
         * Check String Length does not exceed Database VarChar length
         * Compare Password
         */
        [Required(ErrorMessage = "Gebruikersnaam is vereist")] // MUST BE PRESENT
        public string UserName { get; set; }

        [Required(ErrorMessage = "Wachtwoord is vereist")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
    }
}
