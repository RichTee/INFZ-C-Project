using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Database
{
    public class DatabaseController : Controller
    {
        protected MySqlConnection conn;

        public DatabaseController()
        {
            conn = new MySqlConnection("Server=localhost;Port=3306;Database=dierenzaak;Uid=root;Pwd=alpine;");
        }
    }
}
