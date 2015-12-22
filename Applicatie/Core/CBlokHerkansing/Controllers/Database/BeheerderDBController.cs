using CBlokHerkansing.Models.Account;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Database
{
    public class BeheerderDBController : AuthDBController
    {
        public BeheerderDBController() { }


        // Get alle klanten
        public List<Klant> GetKlanten()
        {
            List<Klant> klanten = new List<Klant>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT gebruikerId, email, voornaam, achternaam, wachtwoord, telefoonnummer, goldStatus, r.rol FROM gebruiker g join rol r on g.rolId = r.rolId;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        Klant klant = GetKlantFromDataReader(dataReader);
                        klanten.Add(klant);
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write("Ophalen van klanten mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return klanten;
        }
    }
}