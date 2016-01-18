using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Models.Product
{
    public class ProductDetail
    {
        public int detailId { get; set; }

        [Required(ErrorMessage = "verkoopprijs is vereist")]
        [Range(0, Double.MaxValue, ErrorMessage = "Vul een geldige waarde in")]
        public double verkoopprijs { get; set; }

        [Required(ErrorMessage = "inkoopprijs is vereist")]
        [Range(0, Double.MaxValue, ErrorMessage = "Vul een geldige waarde in")]
        public double inkoopprijs { get; set; }

        [Required(ErrorMessage = "maat is vereist")]
        public int maat { get; set; } // FK?

        [Required(ErrorMessage = "voorraad is vereist")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Vul een geldige waarde in")]
        public int voorraad { get; set; }

        public ProductBase product { get; set; }
    }
}