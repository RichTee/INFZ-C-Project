using CBlokHerkansing.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBlokHerkansing.Controllers.Database
{
    public class CategorieDbController : DatabaseController
    {
        public void insertCategorie(Categorie categorie)
        {
            try
            {
                conn.Open();
                string insertString = @"insert into categorie (naam, omschrijving, hoofdId) " +
                                        "values (@naam, @omschrijving, @hoofdId)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter naamParam = new MySqlParameter("@naam", MySqlDbType.VarChar);
                MySqlParameter omschrijvingParam = new MySqlParameter("@omschrijving", MySqlDbType.VarChar);
                MySqlParameter hoofdIdPercentageParam = new MySqlParameter("@hoofdId", MySqlDbType.Int32);

                naamParam.Value = categorie.Naam;
                omschrijvingParam.Value = categorie.Omschrijving;
                hoofdIdPercentageParam.Value = categorie.HoofdcategorieId;

                cmd.Parameters.Add(naamParam);
                cmd.Parameters.Add(omschrijvingParam);
                cmd.Parameters.Add(hoofdIdPercentageParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Categorie niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void UpdateCategorie(Categorie categorie)
        {
            try
            {
                conn.Open();

                string insertString = "Update categorie set naam = @naam, omschrijving = @omschrijving, hoofdId = @hoofdId where categorieId = @categorieId";
                
                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter naamParam = new MySqlParameter("@naam", MySqlDbType.VarChar);
                MySqlParameter omschrijvingParam = new MySqlParameter("@omschrijving", MySqlDbType.VarChar);
                MySqlParameter hoofdIdPercentageParam = new MySqlParameter("@hoofdId", MySqlDbType.Int32);
                MySqlParameter idParam = new MySqlParameter("@categorieId", MySqlDbType.Int32);

                naamParam.Value = categorie.Naam;
                omschrijvingParam.Value = categorie.Omschrijving;
                hoofdIdPercentageParam.Value = categorie.HoofdcategorieId;
                idParam.Value = categorie.Id;
                cmd.Parameters.Add(idParam);
                cmd.Parameters.Add(naamParam);
                cmd.Parameters.Add(omschrijvingParam);
                cmd.Parameters.Add(hoofdIdPercentageParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Categorie niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteCategorie(int categorieId)
        {
            try
            {
                conn.Open();

                string insertString = "DELETE FROM categorie WHERE Id = @id";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32);

                idParam.Value = categorieId;
                cmd.Parameters.Add(idParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Categorie niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<Categorie> getListWithSubCategorieen(int categorieId)
        {
            List<Categorie> categorieen = new List<Categorie>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT categorieId, naam, omschrijving FROM categorie where hoofdId = @hoofdId;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32);
                idParam.Value = categorieId;
                cmd.Parameters.Add(idParam);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        Categorie categorie = getFullCategorieFromDataReader(dataReader);
                        categorieen.Add(categorie);
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
            return categorieen;
        }
        public List<Categorie> getListWithAllCategorieen(){
        List<Categorie> categorieen = new List<Categorie>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT categorieId, naam, omschrijving FROM categorie;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        Categorie categorie = getFullCategorieFromDataReader(dataReader);
                        categorieen.Add(categorie);
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

            return categorieen;
    }
        public Categorie getCategorieFromId(int categorieId)
        {
            Categorie categorie = new Categorie();
            try
            {
                conn.Open();

                string selectQuery = "SELECT naam, omschrijving, hoofdId FROM categorie where categorieId = @categorieId;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter categorieIdParam = new MySqlParameter("@categorieId", MySqlDbType.Int32);
                categorieIdParam.Value = categorieId;
                cmd.Parameters.Add(categorieIdParam);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        categorie = getFullCategorieFromDataReader(dataReader);
                        categorie.Id = categorieId;
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write("Ophalen van categorie mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return categorie;
        }
        public int getCategorieIdByName(string search)
        {
            int categorieId = 0;
            try
            {
                conn.Open();

                string selectQuery = "SELECT categorieId FROM categorie where naam like %@search%;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter searchParam = new MySqlParameter("@search", MySqlDbType.VarChar);
                searchParam.Value = search;
                cmd.Parameters.Add(searchParam);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        categorieId = dataReader.GetInt32("categorieId");
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
            return categorieId;  
        }
    }
}