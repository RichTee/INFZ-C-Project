using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBlokHerkansing.Models.Account;
using MySql.Data.MySqlClient;
namespace CBlokHerkansing.Controllers.Database
{
    /*To do: 
    Implement roll create and insert statement.
    Password encrypte
     */
    public class AccountDBController : AuthDBController
    {
        // Gebruiker toevoegen
        public void InsertKlant(RegistrationModel registrationModel)
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

                // To Do:
                // De goede MySqlDbType moet hier nog worden aangegeven
                MySqlParameter goldStatusParam = new MySqlParameter("@goldStatus", MySqlDbType.VarChar);
                MySqlParameter telefoonnummerParam = new MySqlParameter("@telefoonnummer", MySqlDbType.VarChar);

                voornaamParam.Value = registrationModel.Voornaam;
                achternaamParam.Value = registrationModel.Achternaam;
                wachtwoordParam.Value = registrationModel.Wachtwoord;
                emailParam.Value = registrationModel.Email;


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

        public Boolean checkGebruikerDuplicaat(string email)
        {
            try
            {
                conn.Open();

                String insertString = "select * from gebruiker where email = @email";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter emailParam = new MySqlParameter("@email", MySqlDbType.VarChar);

                emailParam.Value = email;

                cmd.Parameters.Add(emailParam);
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
