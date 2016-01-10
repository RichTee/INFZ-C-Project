using CBlokHerkansing.Authorisation;
using CBlokHerkansing.Models.Product;
using CBlokHerkansing.ViewModels.Product;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Database
{
    public class ProductDBController : DatabaseController
    {
        public ProductDBController() { }

        // Get alle producten zonder details
        public List<ProductBase> GetProducten()
        {
            List<ProductBase> producten = new List<ProductBase>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT productId, naam, omschrijving, categorieId FROM product;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        ProductBase productBase = GetProductFromDataReader(dataReader);
                        producten.Add(productBase);
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

            return producten;
        }

        // Get 1 product met details
        public ProductDetail GetProductAndDetail(int id)
        {
            try{                
            conn.Open();

            string selectQuery = "SELECT detailId, verkoopprijs, inkoopprijs, maat, kleur, voorraad, naam, omschrijving, categorieId, productId FROM productdetail LEFT JOIN product on productdetail.productId = product.productId;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    return getFullProductFromDataReader(dataReader);
                }
                else
                {
                    return null;
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
        }

        // Insert aanbieding
        public void InsertProduct(ProductBase product)
        {
            try
            {
                conn.Open();
                string insertString = @"insert into product (naam, omschrijving, categorieId) " +
                                        "values (@naam, @omschrijving, @categorieId)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter naamParam = new MySqlParameter("@naam", MySqlDbType.VarChar);
                MySqlParameter omschrijvingParam = new MySqlParameter("@omschrijving", MySqlDbType.VarChar);
                MySqlParameter categorieIdParam = new MySqlParameter("@categorieId", MySqlDbType.Int32);

                naamParam.Value = product.Naam;
                omschrijvingParam.Value = product.Omschrijving;
                categorieIdParam.Value = product.CategorieId;

                cmd.Parameters.Add(naamParam);
                cmd.Parameters.Add(omschrijvingParam);
                cmd.Parameters.Add(categorieIdParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Product niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        // Update 1 product met details
        public ProductDetail UpdateProductAndDetail(ProductDetail productDetail)
        {
            try
            {
                conn.Open();

                string selectQuery = @"Update verkoopprijs, inkoopprijs, maat, kleur, voorraad, naam, omschrijving, FROM productdetail LEFT JOIN product on productdetail.productId = product.productId;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    return getFullProductFromDataReader(dataReader);
                }
                else
                {
                    return null;
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
        }

        public void VerwijderProduct(int id)
        {
            try
            {
                conn.Open();

                string insertString = @"delete from product where productId=@productId";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter productIdParam = new MySqlParameter("@productId", MySqlDbType.Int32);

                productIdParam.Value = id;

                cmd.Parameters.Add(productIdParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Product niet verwijderd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<ProductBase> getListProductFromCategorie(int categorieId)
        {
            
            List<ProductBase> producten = new List<ProductBase>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT productId, naam, omschrijving from product where categorieId = @categorieId";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter categorieIdParam = new MySqlParameter("@categorieId", MySqlDbType.Int32);
                categorieIdParam.Value = categorieId;
                cmd.Parameters.Add(categorieIdParam);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        ProductBase productBase = GetProductFromDataReader(dataReader);
                        producten.Add(productBase);
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

            return producten;
        }
        public List<ProductBase> getListProductFromNameSearch(string search)
        {

            List<ProductBase> producten = new List<ProductBase>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT productId, naam, omschrijving from product where naam like %@search%";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter naamParam = new MySqlParameter("@search", MySqlDbType.VarChar);
                naamParam.Value = search;
                cmd.Parameters.Add(naamParam);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        ProductBase productBase = GetProductFromDataReader(dataReader);
                        producten.Add(productBase);
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

            return producten;
        }
    }
}