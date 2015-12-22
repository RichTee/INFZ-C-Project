using CBlokHerkansing.Models.Account;
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

        // Haal overzicht van klanten binnen
        protected Klant GetKlantFromDataReader(MySqlDataReader dataReader)
        {
            int klantId = dataReader.GetInt32("gebruikerId");
            string klantEmail = dataReader.GetString("email");
            string klantWachtwoord = dataReader.GetString("wachtwoord");
            string klantVoornaam = dataReader.GetString("voornaam");
            string klantAchternaam = dataReader.GetString("achternaam");
            string klantTelefoonnummer = String.IsNullOrEmpty(dataReader["telefoonnummer"].ToString()) ? "Geen" : dataReader.GetString("telefoonnummer");
            string klantGoldStatus = String.IsNullOrEmpty(dataReader["goldStatus"].ToString()) ? "Geen" : dataReader.GetString("goldStatus");
            string klantRechten = dataReader.GetString("rol");
            /*
             * 
             * Adres gegevens indien beschikbaar ook hier
             * 
             */

            Klant klant = new Klant
            {
                Id = klantId,
                Email = klantEmail,
                Wachtwoord = klantWachtwoord,
                Voornaam = klantVoornaam,
                Achternaam = klantAchternaam,
                Telefoonnummer = klantTelefoonnummer,
                GoldStatus = klantGoldStatus,
                Rol = klantRechten
            };

            return klant;
        }
    }
}
