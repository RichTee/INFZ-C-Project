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

        public bool isAuthorized(string email, string password)
        {
            try
            {
                conn.Open();

                string selectQueryStudent = @"select * from gebruiker where email = @email and wachtwoord = @wachtwoord";

                MySqlCommand cmd = new MySqlCommand(selectQueryStudent, conn);
                MySqlParameter emailParam = new MySqlParameter("@email", MySqlDbType.VarChar);
                MySqlParameter passwordParam = new MySqlParameter("@wachtwoord", MySqlDbType.VarChar);

                emailParam.Value = email;
                passwordParam.Value = password;

                cmd.Parameters.Add(emailParam);
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

        // TODO: Nullcheck voor als er geen rollen zijn
        public string[] getRollen(string email)
        {
            try
            {
                conn.Open();

                string selectQuery = @"SELECT rol FROM rol r WHERE r.rolId = (SELECT g.rolId FROM gebruiker g WHERE email = @email);";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter emailParam = new MySqlParameter("@email", MySqlDbType.VarChar);

                emailParam.Value = email;

                cmd.Parameters.Add(emailParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();

                List<string> rollen = new List<string>();
                while (dataReader.Read())
                {
                    string rolnaam = dataReader.GetString("rol");
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
