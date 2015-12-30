using CBlokHerkansing.Models.Account;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Database
{
    public class KlantDBController : AuthDBController
    {
        public KlantDBController() { }

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

        // Get 1 klant
        public Klant GetKlantInformatie(string usr)
        {
            if (string.IsNullOrEmpty(usr))
                return null;

            Klant klant = new Klant();
            try
            {
                conn.Open();

                string selectQuery = "SELECT gebruikerId, email, voornaam, achternaam, wachtwoord, telefoonnummer, goldStatus, r.rol FROM gebruiker g join rol r on g.rolId = r.rolId WHERE email = @email;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter emailParam = new MySqlParameter("@email", MySqlDbType.VarChar);

                emailParam.Value = usr;

                cmd.Parameters.Add(emailParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    klant = GetKlantFromDataReader(dataReader);
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van klant mislukt " + e); // TODO: ViewBag message
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return klant;
        }

        // Update 1 klant
        public void UpdateKlant(Klant klant)
        {
            try
            {
                conn.Open();

                string insertString = @"UPDATE gebruiker SET email=@klant_email, wachtwoord=@klant_wachtwoord, voornaam=@klant_voornaam, achternaam=@klant_achternaam, telefoonnummer=@klant_telefoon WHERE gebruikerId=@klant_id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter klantEmailParam = new MySqlParameter("@klant_email", MySqlDbType.VarChar);
                MySqlParameter klantWachtwoordParam = new MySqlParameter("@klant_wachtwoord", MySqlDbType.VarChar);
                MySqlParameter klantVoornaamParam = new MySqlParameter("@klant_voornaam", MySqlDbType.VarChar);
                MySqlParameter klantAchternaamParam = new MySqlParameter("@klant_achternaam", MySqlDbType.VarChar);
                MySqlParameter klantTelefoonParam = new MySqlParameter("@klant_telefoon", MySqlDbType.VarChar);
                MySqlParameter klantIdParam = new MySqlParameter("@klant_id", MySqlDbType.Int32);

                klantEmailParam.Value = klant.Email;
                klantWachtwoordParam.Value = klant.Wachtwoord;
                klantVoornaamParam.Value = klant.Voornaam;
                klantAchternaamParam.Value = klant.Achternaam;
                klantTelefoonParam.Value = klant.Telefoonnummer;
                klantIdParam.Value = klant.Id;

                cmd.Parameters.Add(klantEmailParam);
                cmd.Parameters.Add(klantWachtwoordParam);
                cmd.Parameters.Add(klantVoornaamParam);
                cmd.Parameters.Add(klantAchternaamParam);
                cmd.Parameters.Add(klantTelefoonParam);
                cmd.Parameters.Add(klantIdParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.Write("Updaten klant niet gelukt: " + e); // TODO: ViewBag message
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}