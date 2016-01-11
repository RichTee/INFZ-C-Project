using CBlokHerkansing.Models.Klant;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Controllers.Database
{
    public class AdresDBController : DatabaseController
    {
        public void InsertAdres(Adres adres)
        {
            try
            {
                conn.Open();
                string insertString = @"insert into adres (straat, postcode, huisnummer,huisnummertoevoegsel,stad, gebruikerId) " +
                                        "values (@straat, @postcode, @huisnummer, @huisnummertoevoegsel, @stad, @gebruikerId)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter straatParam = new MySqlParameter("@straat", MySqlDbType.VarChar);
                MySqlParameter postcodeParam = new MySqlParameter("@postcode", MySqlDbType.VarChar);
                MySqlParameter huisnummerParam = new MySqlParameter("@huisnummer", MySqlDbType.Int32);
                MySqlParameter huisnummertoevoegselParam = new MySqlParameter("@huisnummertoevoegsel", MySqlDbType.VarChar);
                MySqlParameter stadParam = new MySqlParameter("@stad", MySqlDbType.VarChar);
                MySqlParameter gebruikerIdParam = new MySqlParameter("@gebruikerId", MySqlDbType.Int32);

                straatParam.Value = adres.Straat;
                postcodeParam.Value = adres.Postcode;
                huisnummerParam.Value = adres.Huisnummer;
                huisnummertoevoegselParam.Value = adres.HuisnummerToevoegsel;
                stadParam.Value = adres.Stad;
                gebruikerIdParam.Value = adres.GebruikerId;

                cmd.Parameters.Add(straatParam);
                cmd.Parameters.Add(postcodeParam);
                cmd.Parameters.Add(huisnummerParam);
                cmd.Parameters.Add(huisnummertoevoegselParam);
                cmd.Parameters.Add(stadParam);
                cmd.Parameters.Add(gebruikerIdParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Adres niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void UpdateAdres(Adres adres)
        {
            try
            {
                conn.Open();

                string insertString = "Update Adres set straat = @straat, postcode = @postcode, huisnummer = @huisnummer, huisnummertoevoegsel = @huisnummertoevoegsel, stad = @stad, gebruikerId = @gebruikerId where adresId = @adresId";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter straatParam = new MySqlParameter("@straat", MySqlDbType.VarChar);
                MySqlParameter postcodeParam = new MySqlParameter("@postcode", MySqlDbType.VarChar);
                MySqlParameter huisnummerParam = new MySqlParameter("@huisnummer", MySqlDbType.Int32);
                MySqlParameter huisnummertoevoegselParam = new MySqlParameter("@huisnummertoevoegsel", MySqlDbType.VarChar);
                MySqlParameter stadParam = new MySqlParameter("@stad", MySqlDbType.VarChar);
                MySqlParameter gebruikerIdParam = new MySqlParameter("@gebruikerId", MySqlDbType.Int32);
                MySqlParameter adresIdParam = new MySqlParameter("@adresId", MySqlDbType.Int32);

                straatParam.Value = adres.Straat;
                postcodeParam.Value = adres.Postcode;
                huisnummerParam.Value = adres.Huisnummer;
                huisnummertoevoegselParam.Value = adres.HuisnummerToevoegsel;
                stadParam.Value = adres.Stad;
                gebruikerIdParam.Value = adres.GebruikerId;
                adresIdParam.Value = adres.Id;

                cmd.Parameters.Add(straatParam);
                cmd.Parameters.Add(postcodeParam);
                cmd.Parameters.Add(huisnummerParam);
                cmd.Parameters.Add(huisnummertoevoegselParam);
                cmd.Parameters.Add(stadParam);
                cmd.Parameters.Add(gebruikerIdParam);
                cmd.Parameters.Add(adresIdParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Categorie niet veranderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteAdres(int adresId)
        {
            try
            {
                conn.Open();

                string insertString = "DELETE FROM adres WHERE adresId = @id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter adresidParam = new MySqlParameter("@id", MySqlDbType.Int32);

                adresidParam.Value = adresId;
                cmd.Parameters.Add(adresidParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Adres niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<Adres> getListWithAdresForCustomer(int gebruikerId)
        {
            List<Adres> adressen = new List<Adres>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT * FROM adres where gebuikerId = @gebuikerId;";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter gebruikerIdParam = new MySqlParameter("@gebuikerId", MySqlDbType.Int32);
                gebruikerIdParam.Value = gebruikerId;
                cmd.Parameters.Add(gebruikerIdParam);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        Adres adres = getFullAdresFromDataReader(dataReader);
                        adressen.Add(adres);
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write("Ophalen van producten mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return adressen;
        }
    }
}