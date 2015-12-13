using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Database
{
    public class AuthDBController : DatabaseController
    {

        public bool isAuthorized(string username, string password)
        {
            try
            {
                conn.Open();

                string selectQueryStudent = @"select * from gebruiker where gebruikersnaam = @gebruikersnaam and wachtwoord = @wachtwoord";

                MySqlCommand cmd = new MySqlCommand(selectQueryStudent, conn);

                MySqlParameter usernameParam = new MySqlParameter("@gebruikersnaam", MySqlDbType.VarChar);
                MySqlParameter passwordParam = new MySqlParameter("@wachtwoord", MySqlDbType.VarChar);

                usernameParam.Value = username;
                passwordParam.Value = password;

                cmd.Parameters.Add(usernameParam);
                cmd.Parameters.Add(passwordParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                return dataReader.Read();

            }
            catch (Exception e)
            {
                Console.WriteLine(e); // Show error message to user
                return false;
            }
            finally
            {
                conn.Close();
            }

        }

        public string[] getRollen(string username)
        {
            try
            {
                conn.Open();

                string selectQueryStudent = @"select rechten 
                                                from klant
                                                where gebruikersnaam = @username;";

                MySqlCommand cmd = new MySqlCommand(selectQueryStudent, conn);

                MySqlParameter usernameParam = new MySqlParameter("@username", MySqlDbType.VarChar);
                usernameParam.Value = username;
                cmd.Parameters.Add(usernameParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                List<string> rollen = new List<string>();
                while (dataReader.Read())
                {
                    string rolnaam = dataReader.GetString("rechten");
                    rollen.Add(rolnaam);
                }

                return rollen.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e); // Show error message to users
                return null;
            }
            finally
            {
                conn.Close();
            }

        }
    }
}