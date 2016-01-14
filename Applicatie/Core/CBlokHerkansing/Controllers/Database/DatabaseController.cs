using CBlokHerkansing.Models;
using CBlokHerkansing.Models.Account;
using CBlokHerkansing.Models.Bestelling;
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
        {            try
            {
                conn = new MySqlConnection("Server=localhost;Port=3306;Database=dierenzaak;Uid=root;Pwd=root;");
            }
            catch (Exception e)
            {
                conn = new MySqlConnection("Server=85.214.97.236; Database=CBlokHerkansing;Uid=dierenwinkel;Pwd=Ear98@8g;");
            }
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

        // Haal Product & Detail binnen
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

        protected ProductDetail getProductDetailFromDataReader(MySqlDataReader datareader)
        {

            ProductDetail productDetail = new ProductDetail
            {
                detailId = datareader.GetInt32("detailID"),
                verkoopprijs = datareader.GetDouble("verkoopprijs"),
                inkoopprijs = datareader.GetDouble("inkoopprijs"),
                maat = datareader.GetInt32("maat"),
                kleur = datareader.GetString("kleur"),
                voorraad = datareader.GetInt32("voorraad"),
            };
            return productDetail;
        }

        protected Categorie getFullCategorieFromDataReader(MySqlDataReader datareader)
        {
            Categorie categorie = new Categorie
            {
                Id = datareader.GetInt32("categorieId"),
                Naam = datareader.GetString("naam"),
                Omschrijving = datareader.GetString("omschrijving")
            };
            return categorie;
        }
        protected Adres getFullAdresFromDataReader(MySqlDataReader datareader)
        {
            Adres adres = new Adres
            {
                Id = datareader.GetInt32("adresId"),
                Straat = datareader.GetString("straat"),
                Postcode = datareader.GetString("postcode"),
                Huisnummer = datareader.GetInt32("huisnummer"),
                HuisnummerToevoegsel = String.IsNullOrEmpty(datareader["huisnummertoevoeging"].ToString()) ? "Geen" : datareader.GetString("huisnummertoevoeging"),
                Stad = datareader.GetString("stad"),
            };
            return adres;
        }

        /*
         * 
         * Bestelling
         * 
         */
        protected BestellingBase getBestellingFromDataReader(MySqlDataReader datareader)
        {
            BestellingBase bestellingBase = new BestellingBase
            {
                BestellingId = datareader.GetInt32("bestellingId"),
                BezorgStatus = datareader.GetString("bezorgStatus"),
                BezorgTijd = datareader.GetString("bezorgTijd"),
                BestelDatum = datareader.GetString("bestelDatum"),
                Adres = getFullAdresFromDataReader(datareader),
                Gebruiker = GetKlantFromDataReader(datareader),
            };
            return bestellingBase;
        }

        protected BestelRegel getFullBestellingFromDataReader(MySqlDataReader datareader)
        {
            BestelRegel bestellingRegel = null;
            if (GetIntValue(datareader, "aanbiedingId") == 0)
            {
                ProductAanbieding productAanbieding = new ProductAanbieding();
                bestellingRegel = new BestelRegel
                {
                    RegelId = datareader.GetInt32("regelId"),
                    Hoeveelheid = datareader.GetInt32("hoeveelheid"),
                    Datum = datareader.GetDateTime("datum") == null ? "geen" : datareader.GetDateTime("datum").ToString("yyyy/M/%d"),
                    Bestelling = getBestellingFromDataReader(datareader),
                    ProductDetail = getProductDetailFromDataReader(datareader),
                    ProductAanbieding = productAanbieding,
                };
            }
            else
            {
                bestellingRegel = new BestelRegel
                {
                    RegelId = datareader.GetInt32("regelId"),
                    Hoeveelheid = datareader.GetInt32("hoeveelheid"),
                    Datum = datareader.GetDateTime("datum") == null ? "geen" : datareader.GetDateTime("datum").ToString("yyyy/M/%d"),
                    Bestelling = getBestellingFromDataReader(datareader),
                    ProductDetail = getProductDetailFromDataReader(datareader),
                    ProductAanbieding = GetAanbiedingFromDataReader(datareader),
                };
            }

            return bestellingRegel;
        }

        // Check if int DB value is DBNull or null
        private int GetIntValue(MySqlDataReader reader, String columnName)
        {
            return (reader[columnName] != null && reader[columnName] != DBNull.Value) ? Convert.ToInt32(reader[columnName]) : 0;
        }
    }
}
