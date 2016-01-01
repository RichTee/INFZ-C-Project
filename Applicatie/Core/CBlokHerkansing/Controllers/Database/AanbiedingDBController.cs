using CBlokHerkansing.Models.Product;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Database
{
    public class AanbiedingDBController : DatabaseController
    {
        public AanbiedingDBController() {}

        // Get alle aanbiedingen zonder details
        public List<ProductAanbieding> GetAanbiedingen()
        {
            List<ProductAanbieding> aanbiedingen = new List<ProductAanbieding>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT aanbiedingId, beginDatum, eindDatum, kortingsPercentage FROM aanbieding;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        ProductAanbieding aanbieding = GetAanbiedingFromDataReader(dataReader);
                        aanbiedingen.Add(aanbieding);
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write("Ophalen van aanbiedingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return aanbiedingen;
        }
    }
}