using CBlokHerkansing.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.ViewModels.Account
{
    public class ManagerViewModel
    {
        public List<ProductBase> bestSeller { get; set; }
        public List<ProductBase> worstSeller { get; set; }
        public double omzet { get; set; }
        public double winst { get; set; }
    }
}