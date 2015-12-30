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
        public ProductViewModel GetProductAndDetail(int id)
        {
            // TODO: Get Product & Details
            return null;
        }

        // Update 1 product met details
        public ProductViewModel UpdateProductAndDetail(ProductViewModel viewModel)
        {
            if (viewModel == null)
                return null;

            // TODO: Update Product & Details

            return null;
        }
    }
}