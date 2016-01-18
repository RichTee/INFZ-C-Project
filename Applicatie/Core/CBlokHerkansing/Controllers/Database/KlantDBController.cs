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

        // check if gebruiker is eligible for gold member
        public bool CheckGebruikerGoldMember(int id)
        {
            try
            {
                conn.Open();

                String insertString = @"SELECT 
                                        SUM(pd.verkoopprijs * br.hoeveelheid) as MemberCheck 
                                        FROM bestelling be 
                                        JOIN bestelRegel br on br.bestelId = be.bestellingId
                                        JOIN productDetail pd on pd.detailId = br.detailId
                                        JOIN gebruiker gb on gb.gebruikerId = be.gebruikerId
                                        WHERE be.gebruikerId = @gebruikerId AND gb.goldStatus IS NULL
                                        HAVING MemberCheck >= 400;";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter gebruikerIdParam = new MySqlParameter("@gebruikerId", MySqlDbType.Int32);

                gebruikerIdParam.Value = id;

                cmd.Parameters.Add(gebruikerIdParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                    return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return false;
        }

        public bool CheckKlantMaxAdres(int id)
        {
            try
            {
                conn.Open();

                String insertString = "SELECT count(*) as count FROM adres WHERE gebruikerId = @gebruikerId;";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter gebruikerIdParam = new MySqlParameter("@gebruikerId", MySqlDbType.Int32);

                gebruikerIdParam.Value = id;

                cmd.Parameters.Add(gebruikerIdParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    if (3 == dataReader.GetInt32("count"))
                        return true;
                }
            }
            catch (Exception e)
            {
                throw e; // TODO: Show exception to user via Viewbag
            }
            finally
            {
                conn.Close();
            }

            return false;
        }

        // Retrieve klant id met Email
        public int GetKlantId(string email)
        {
            int klantId = 0;
            try
            {
                conn.Open();

                string selectQuery = "SELECT gebruikerId FROM gebruiker WHERE email = @email;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter emailParam = new MySqlParameter("@email", MySqlDbType.VarChar);

                emailParam.Value = email;

                cmd.Parameters.Add(emailParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                    klantId = dataReader.GetInt32("gebruikerId");
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

            return klantId;
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

        /*
         * Adres
         * 
         */
        public bool UserHasAdres(int id, string email)
        {
            try
            {
                conn.Open();

                String insertString = "SELECT adresId FROM adres ad WHERE adresId = @adresId AND ad.gebruikerId = (SELECT gr.gebruikerId FROM gebruiker gr WHERE gr.email = @email);";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter adresIdParam = new MySqlParameter("@adresId", MySqlDbType.VarChar);
                MySqlParameter emailParam = new MySqlParameter("@email", MySqlDbType.VarChar);

                adresIdParam.Value = id;
                emailParam.Value = email;

                cmd.Parameters.Add(adresIdParam);
                cmd.Parameters.Add(emailParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                throw e; // TODO: Show exception to user via Viewbag
            }
            finally
            {
                conn.Close();
            }
        }
        public int GetAdresId(int id)
        {
            int adresId = 0;
            try
            {
                conn.Open();

                string selectQuery = "SELECT adresId FROM adres WHERE gebruikerId = @gebruikerId;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter gebruikerIdParam = new MySqlParameter("@gebruikerId", MySqlDbType.Int32);

                gebruikerIdParam.Value = id;

                cmd.Parameters.Add(gebruikerIdParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                    adresId = dataReader.GetInt32("adresId");
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

            return adresId;
        }

        public Adres GetKlantAdresByUserId(int id)
        {
            try
            {
                conn.Open();

                string selectQuery = "SELECT * FROM adres WHERE gebruikerId = @gebruikerId";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter gebruikerIdParam = new MySqlParameter("@gebruikerId", MySqlDbType.VarChar);

                gebruikerIdParam.Value = id;

                cmd.Parameters.Add(gebruikerIdParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                    return getFullAdresFromDataReader(dataReader);
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van adres mislukt " + e); // TODO: ViewBag message
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return null;
        }

        public Adres GetKlantAdresByAdresId(int id)
        {
            try
            {
                conn.Open();

                string selectQuery = "SELECT * FROM adres WHERE adresId = @adresId";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter adresIdParam = new MySqlParameter("@adresId", MySqlDbType.Int32);

                adresIdParam.Value = id;

                cmd.Parameters.Add(adresIdParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Adres adres = getFullAdresFromDataReader(dataReader);
                    adres.GebruikerId = dataReader.GetInt32("gebruikerId");
                    return adres;
                }
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van adres mislukt " + e); // TODO: ViewBag message
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return null;
        }

        public List<Adres> GetKlantAdressen(int id)
        {
            List<Adres> adres = new List<Adres>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT * FROM adres WHERE gebruikerId = @gebruikerId";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter gebruikerIdParam = new MySqlParameter("@gebruikerId", MySqlDbType.VarChar);

                gebruikerIdParam.Value = id;

                cmd.Parameters.Add(gebruikerIdParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                    adres.Add(getFullAdresFromDataReader(dataReader));
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

            return adres;
        }

        /*
         * 
         * CRUD
         * 
         */
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

        /*
         * 
         * adres
         * 
         */
        public void InsertAdres(Adres adres)
        {
            try
            {
                conn.Open();

                // Column                                          1           2          3         4       5           6
                string insertString = @"insert into adres (straat,  postcode, huisnummer, huisnummertoevoeging, stad, gebruikerId) 
                                                        values (@straat,@postcode, @huisnummer, @huisnummertoevoeging, @stad, @gebruikerId)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter straatParam = new MySqlParameter("@straat", MySqlDbType.VarChar);
                MySqlParameter postcodeParam = new MySqlParameter("@postcode", MySqlDbType.VarChar);
                MySqlParameter huisnummerParam = new MySqlParameter("@huisnummer", MySqlDbType.VarChar);
                MySqlParameter huisnummerToevoegingParam = new MySqlParameter("@huisnummertoevoeging", MySqlDbType.VarChar);
                MySqlParameter stadParam = new MySqlParameter("@stad", MySqlDbType.VarChar);
                MySqlParameter gebruikerIdParam = new MySqlParameter("@gebruikerId", MySqlDbType.Int32);


                straatParam.Value = adres.Straat;
                postcodeParam.Value = adres.Postcode;
                huisnummerParam.Value = adres.Huisnummer;
                if (string.IsNullOrEmpty(adres.HuisnummerToevoegsel))
                    huisnummerToevoegingParam.Value = adres.HuisnummerToevoegsel;
                else
                    huisnummerToevoegingParam.Value = DBNull.Value;
                stadParam.Value = adres.Stad;
                gebruikerIdParam.Value = adres.GebruikerId;

                cmd.Parameters.Add(straatParam);
                cmd.Parameters.Add(postcodeParam);
                cmd.Parameters.Add(huisnummerParam);
                cmd.Parameters.Add(huisnummerToevoegingParam);
                cmd.Parameters.Add(stadParam);
                cmd.Parameters.Add(gebruikerIdParam);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Klant adres niet toegevoegd: " + e); // TODO: Show exception to user via Viewbag
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        // Update 1 klant
        public void UpdateAdres(Adres adres)
        {
            try
            {
                conn.Open();

                string insertString = @"UPDATE adres SET straat=@straat, postcode=@postcode, huisnummer=@huisnummer, huisnummertoevoeging=@huisnummertoevoeging, stad=@stad, gebruikerId=@gebruikerId WHERE adresId = @adresId";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter adresStraatParam = new MySqlParameter("@straat", MySqlDbType.VarChar);
                MySqlParameter adresPostcodeParam = new MySqlParameter("@postcode", MySqlDbType.VarChar);
                MySqlParameter adresHuisnummerParam = new MySqlParameter("@huisnummer", MySqlDbType.VarChar);
                MySqlParameter adresHuisnummerToevoegingParam = new MySqlParameter("@huisnummertoevoeging", MySqlDbType.VarChar);
                MySqlParameter adresStadParam = new MySqlParameter("@stad", MySqlDbType.VarChar);
                MySqlParameter gebruikerIdParam = new MySqlParameter("gebruikerId", MySqlDbType.Int32);
                MySqlParameter adresIdParam = new MySqlParameter("@adresId", MySqlDbType.Int32);

                adresStraatParam.Value = adres.Straat;
                adresPostcodeParam.Value = adres.Postcode;
                adresHuisnummerParam.Value = adres.Huisnummer;
                adresHuisnummerToevoegingParam.Value = adres.HuisnummerToevoegsel;
                adresStadParam.Value = adres.Stad;
                gebruikerIdParam.Value = adres.GebruikerId;
                adresIdParam.Value = adres.Id;

                cmd.Parameters.Add(adresStraatParam);
                cmd.Parameters.Add(adresPostcodeParam);
                cmd.Parameters.Add(adresHuisnummerParam);
                if (!string.IsNullOrEmpty(adres.HuisnummerToevoegsel))
                    cmd.Parameters.Add(adresHuisnummerToevoegingParam);
                else
                    cmd.Parameters.Add(DBNull.Value);
                cmd.Parameters.Add(adresStadParam);
                cmd.Parameters.Add(gebruikerIdParam);
                cmd.Parameters.Add(adresIdParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.Write("Updaten klant adres niet gelukt: " + e); // TODO: ViewBag message
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void VerwijderAdres(int id)
        {
            try
            {
                conn.Open();

                string insertString = @"delete from adres where adresId=@adresId";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter adresIdParam = new MySqlParameter("@adresId", MySqlDbType.Int32);

                adresIdParam.Value = id;

                cmd.Parameters.Add(adresIdParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Klant adres niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        /*
         * 
         * Membership
         * 
         */
        public void UpdateGoldMember(int user)
        {
            try
            {
                conn.Open();

                string insertString = @"UPDATE gebruiker SET goldStatus = @goldStatus WHERE gebruikerId=@gebruikerId";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter goldStatusParam = new MySqlParameter("@goldStatus", MySqlDbType.VarChar);
                MySqlParameter gebruikerIdParam = new MySqlParameter("@gebruikerId", MySqlDbType.Int32);

                goldStatusParam.Value = "Ja";
                gebruikerIdParam.Value = user;

                cmd.Parameters.Add(goldStatusParam);
                cmd.Parameters.Add(gebruikerIdParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.Write("Updaten membership niet gelukt: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}