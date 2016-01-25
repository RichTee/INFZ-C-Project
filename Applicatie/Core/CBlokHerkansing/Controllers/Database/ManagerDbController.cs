using CBlokHerkansing.Controllers.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Controllers.Manager
{
    public class ManagerDbController : DatabaseController
    {
        public List<int> getBestSellers()
        {
            List<int> list = new List<int>();
            try
            {
                conn.Open();

                string selectQuery = "select detailId, Count(Distinct detailId) AS amount from bestelregel order by amount asc limit 10;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        list.Add(dataReader.GetInt32("detailId"));
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
            return list;
        }
        public List<int> getWorsttSellers()
        {
            List<int> list = new List<int>();
            try
            {
                conn.Open();

                string selectQuery = "select detailId, Count(Distinct detailId) AS amount from bestelregel order by amount desc limit 10;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        list.Add(dataReader.GetInt32("detailId"));
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
            return list;
        }

        // Get Omzet
        public double getOmzet()
        {           
            try
            {
                conn.Open();

                string selectQuery = @"SELECT 
                                        SUM(DISTINCT pd.verkoopprijs * br.hoeveelheid) AS Amount 
                                        FROM productdetail pd 
                                        JOIN bestelRegel br on pd.detailId = br.detailId
                                        JOIN bestelling be on be.bestellingId = br.bestelId
                                        WHERE be.bestelDatum >= DATE_SUB(NOW(), INTERVAL 6 MONTH);";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                        return dataReader.GetDouble("Amount");        
                }

            }
            catch (Exception e)
            {
                Console.Write("Ophalen van omzet mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return 0;
        }
    }
}