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

        /*
        // Update 1 product met details
        public void UpdateProductAndDetail(ProductViewModel viewModel)
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
         * 
        }
         * */
    }
}