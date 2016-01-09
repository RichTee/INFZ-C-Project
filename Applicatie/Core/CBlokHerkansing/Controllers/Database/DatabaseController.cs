using CBlokHerkansing.Models.Account;
using CBlokHerkansing.Models.Klant;
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
            conn = new MySqlConnection("Server=localhost;Port=3306;Database=dierenzaak;Uid=root;Pwd=root;");
        }

        /*
         * 
         * Klant
         * 
         */

        // Haal overzicht van klanten binnen
        protected KlantBase GetKlantFromDataReader(MySqlDataReader dataReader)
        {
            /*
             * 
             * Adres gegevens indien beschikbaar ook hier
             * 
             */

            KlantBase klant = new KlantBase
            {
                Id = dataReader.GetInt32("gebruikerId"),
                Email = dataReader.GetString("email"),
                Wachtwoord = dataReader.GetString("wachtwoord"),
                Voornaam = dataReader.GetString("voornaam"),
                Achternaam = dataReader.GetString("achternaam"),
                Telefoonnummer = String.IsNullOrEmpty(dataReader["telefoonnummer"].ToString()) ? "Geen" : dataReader.GetString("telefoonnummer"),
                GoldStatus = String.IsNullOrEmpty(dataReader["goldStatus"].ToString()) ? "Geen" : dataReader.GetString("goldStatus"),
                Rol = dataReader.GetString("rol")
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
            ProductBase product = new ProductBase
            {
                ProductId = dataReader.GetInt32("productId"),
                Naam = dataReader.GetString("naam"),
                Omschrijving = dataReader.GetString("omschrijving"),
                CategorieId = dataReader.GetInt32("categorieId"),
            };

            return product;
        }

        /*
         * 
         * Aanbieding
         * 
         */

        // Haal overzicht van aanbiedingen binnen
        protected ProductAanbieding GetAanbiedingFromDataReader(MySqlDataReader dataReader)
        {
            ProductAanbieding aanbieding = new ProductAanbieding
            {
                AanbiedingId = dataReader.GetInt32("aanbiedingId"),
                BeginDatum = dataReader.GetDateTime("beginDatum") == null ? "geen" : dataReader.GetDateTime("beginDatum").ToString("yyyy/M/%d"),
                EindDatum = dataReader.GetDateTime("eindDatum") == null ? "geen" : dataReader.GetDateTime("eindDatum").ToString("yyyy/%M/%d"),
                KortingsPercentage = dataReader.GetInt32("kortingsPercentage"),
            };

            return aanbieding;
        }

        protected ProductDetail getFullProductFromDataReader(MySqlDataReader datareader)
        {

            ProductDetail productDetail = new ProductDetail
            {
                detailId = datareader.GetInt32("detailID"),
                verkoopprijs = datareader.GetDouble("verkoopprijs"),
                inkoopprijs = datareader.GetDouble("inkoopprijs"),
                maat = datareader.GetInt32("maat"),
                kleur = datareader.GetString("kleur"),
                voorraad = datareader.GetInt32("voorraad"),
                product = GetProductFromDataReader(datareader)
            };
            return productDetail;
        }

    }
}
