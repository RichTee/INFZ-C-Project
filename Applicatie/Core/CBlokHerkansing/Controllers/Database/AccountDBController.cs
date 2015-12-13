using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBlokHerkansing.Models.Account;
using MySql.Data.MySqlClient;
namespace CBlokHerkansing.Controllers.Database
{
    public class AccountDBController : AuthDBController
    {
        // Gebruiker toevoegen
        public void InsertKlant(RegistrationModel registrationModel)
        {
            try
            {
                conn.Open();

                // KolomNummering                           1               2
                string insertString = @"insert into gebruiker (gebruikersnaam,  wachtwoord) 
                                                        values (@gebruikersnaam,@wachtwoord)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter gebruikersnaamParam = new MySqlParameter("@gebruikersnaam", MySqlDbType.VarChar);
                MySqlParameter wachtwoordParam = new MySqlParameter("@wachtwoord", MySqlDbType.VarChar);

                gebruikersnaamParam.Value = registrationModel.Username;
                wachtwoordParam.Value = registrationModel.Password;

                cmd.Parameters.Add(gebruikersnaamParam);
                cmd.Parameters.Add(wachtwoordParam);
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
        public Boolean checkGebruikerDuplicaat(string gebruikersnaam)
        {
            try
            {
                conn.Open();

                String insertString = "select * from gebruiker where gebruikersnaam = @gebruikersnaam";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter gebruikersnaamParam = new MySqlParameter("@gebruikersnaam", MySqlDbType.VarChar);

                gebruikersnaamParam.Value = gebruikersnaam;

                cmd.Parameters.Add(gebruikersnaamParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (!dataReader.Read())
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
    }
}