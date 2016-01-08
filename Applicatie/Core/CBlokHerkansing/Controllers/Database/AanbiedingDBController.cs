using CBlokHerkansing.Models.Product;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Database
{
    public class AanbiedingDBController : DatabaseController
    {
        public AanbiedingDBController() {}

        // Get 1 aanbieding
        public ProductAanbieding GetAanbieding(int id)
        {
            ProductAanbieding aanbieding = null;

            try
            {
                conn.Open();

                string selectQuery = "SELECT aanbiedingId, beginDatum, eindDatum, kortingsPercentage from aanbieding where aanbiedingId=@aanbiedingId";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter aanbiedingsIdParam = new MySqlParameter("aanbiedingId", MySqlDbType.Int32);

                aanbiedingsIdParam.Value = id;

                cmd.Parameters.Add(aanbiedingsIdParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    aanbieding = GetAanbiedingFromDataReader(dataReader);

                }


            }
            catch (Exception e)
            {

                Console.Write("Ophalen van aanbieding mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return aanbieding;
        }

        // Get alle aanbiedingen
        public List<ProductAanbieding> GetAanbiedingen()
        {
            List<ProductAanbieding> aanbiedingen = new List<ProductAanbieding>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT aanbiedingId, beginDatum, eindDatum, kortingsPercentage FROM aanbieding;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        ProductAanbieding aanbieding = GetAanbiedingFromDataReader(dataReader);
                        aanbiedingen.Add(aanbieding);
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write("Ophalen van aanbiedingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return aanbiedingen;
        }

        // Insert aanbieding
        public void InsertAanbieding(ProductAanbieding aanbieding)
        {
            try
            {
                conn.Open();
                string insertString = @"insert into aanbieding (beginDatum, eindDatum, kortingspercentage) " +
                                        "values (@beginDatum, @eindDatum, @kortingspercentage)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter beginDatumParam = new MySqlParameter("@beginDatum", MySqlDbType.VarChar);
                MySqlParameter eindDatumParam = new MySqlParameter("@eindDatum", MySqlDbType.VarChar);
                MySqlParameter kortingsPercentageParam = new MySqlParameter("@kortingspercentage", MySqlDbType.Int32);

                beginDatumParam.Value = aanbieding.BeginDatum;
                eindDatumParam.Value = aanbieding.EindDatum;
                kortingsPercentageParam.Value = aanbieding.KortingsPercentage;

                cmd.Parameters.Add(beginDatumParam);
                cmd.Parameters.Add(eindDatumParam);
                cmd.Parameters.Add(kortingsPercentageParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Aanbieding niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        // Wijzig 1 aanbieding
        public void WijzigAanbieding(ProductAanbieding aanbieding)
        {
            try
            {
                conn.Open();

                string insertString = @"update aanbieding set beginDatum=@beginDatum, eindDatum=@eindDatum, kortingsPercentage=@kortingspercentage where aanbiedingId=@aanbiedingId";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter aanbiedingIdParam = new MySqlParameter("@aanbiedingId", MySqlDbType.Int32);
                MySqlParameter beginDatumParam = new MySqlParameter("@beginDatum", MySqlDbType.Date);
                MySqlParameter eindDatumParam = new MySqlParameter("@eindDatum", MySqlDbType.Date);
                MySqlParameter kortingspercentageParam = new MySqlParameter("@kortingspercentage", MySqlDbType.Int32);

                aanbiedingIdParam.Value = aanbieding.AanbiedingId;
                beginDatumParam.Value = aanbieding.BeginDatum;
                eindDatumParam.Value = aanbieding.EindDatum;
                kortingspercentageParam.Value = aanbieding.KortingsPercentage;

                cmd.Parameters.Add(aanbiedingIdParam);
                cmd.Parameters.Add(beginDatumParam);
                cmd.Parameters.Add(eindDatumParam);
                cmd.Parameters.Add(kortingspercentageParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Wijzigen aanbieding niet gelukt: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void VerwijderAanbieding(int id)
        {
            try
            {
                conn.Open();

                string insertString = @"delete from aanbieding where aanbiedingId=@aanbiedingId";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter aanbiedingsIdParam = new MySqlParameter("@aanbiedingId", MySqlDbType.Int32);

                aanbiedingsIdParam.Value = id;

                cmd.Parameters.Add(aanbiedingsIdParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Aanbieding niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}