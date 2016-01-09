using CBlokHerkansing.Models.Klant;
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
        public List<KlantBase> GetKlanten()
        {
            List<KlantBase> klanten = new List<KlantBase>();
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
                        KlantBase klant = GetKlantFromDataReader(dataReader);
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
        public KlantBase GetKlantInformatie(string usr)
        {
            if (string.IsNullOrEmpty(usr))
                return null;

            KlantBase klant = new KlantBase();
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

        // Gebruiker toevoegen
        public void InsertKlant(KlantBase klant)
        {
            try
            {
                conn.Open();

                // Column                                          1           2          3         4       5           6
                string insertString = @"insert into gebruiker (voornaam,  achternaam, wachtwoord, email, goldStatus, telefoonnummer) 
                                                        values (@voornaam,@achternaam, @wachtwoord, @email, @goldStatus, @telefoonnummer)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter voornaamParam = new MySqlParameter("@voornaam", MySqlDbType.VarChar);
                MySqlParameter achternaamParam = new MySqlParameter("@achternaam", MySqlDbType.VarChar);
                MySqlParameter wachtwoordParam = new MySqlParameter("@wachtwoord", MySqlDbType.VarChar);
                MySqlParameter emailParam = new MySqlParameter("@email", MySqlDbType.VarChar);
                MySqlParameter goldStatusParam = new MySqlParameter("@goldStatus", MySqlDbType.VarChar);
                MySqlParameter telefoonnummerParam = new MySqlParameter("@telefoonnummer", MySqlDbType.VarChar);

                voornaamParam.Value = klant.Voornaam;
                achternaamParam.Value = klant.Achternaam;
                wachtwoordParam.Value = klant.Wachtwoord;
                emailParam.Value = klant.Email;
                telefoonnummerParam.Value = klant.Telefoonnummer;

                cmd.Parameters.Add(voornaamParam);
                cmd.Parameters.Add(achternaamParam);
                cmd.Parameters.Add(wachtwoordParam);
                cmd.Parameters.Add(emailParam);
                cmd.Parameters.Add(goldStatusParam);
                cmd.Parameters.Add(telefoonnummerParam);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Klant niet toegevoegd: " + e); // TODO: Show exception to user via Viewbag
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        // Update 1 klant
        public void UpdateKlant(KlantBase klant)
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

        public void VerwijderKlant(string email)
        {
            try
            {
                conn.Open();

                string insertString = @"delete from gebruiker where email=@email";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter emailParam = new MySqlParameter("@email", MySqlDbType.VarChar);

                emailParam.Value = email;

                cmd.Parameters.Add(emailParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Klant niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}