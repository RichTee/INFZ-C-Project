using CBlokHerkansing.Models.Account;
using CBlokHerkansing.Models.Product;
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
        // TODO: Enum gebruiken voor datareader.GetString(<ENUM>)
        //                           datareader.GetInt(<ENUM>)
        // Zodat er een centrale plaats is en het sneller te vervangen is indien een kolom naam verandert.
        protected MySqlConnection conn;

        /*
         * 
         * Connection Details
         * 
         */
        public DatabaseController()
        {
            conn = new MySqlConnection("Server=localhost;Port=3306;Database=dierenzaak;Uid=root;Pwd=alpine;");
        }

        /*
         * 
         * Klant
         * 
         */

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

        /*
         * 
         * Product
         * 
         */

        // Haal overzicht van producten binnen
        protected ProductBase GetProductFromDataReader(MySqlDataReader dataReader)
        {
            int productId = dataReader.GetInt32("productId");
            string productNaam = dataReader.GetString("naam");
            string productOmschrijving = dataReader.GetString("omschrijving");
            int categorieId = dataReader.GetInt32("categorieId");
            /*
             * 
             * Adres gegevens indien beschikbaar ook hier
             * 
             */

            ProductBase product = new ProductBase
            {
                ProductId = productId,
                Naam = productNaam,
                Omschrijving = productOmschrijving,
                CategorieId = categorieId,
            };

            return product;
        }
    }
}
