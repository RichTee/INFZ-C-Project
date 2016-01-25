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
            foreach (ProductBase product in producten)
            {
                product.AfbeeldingPath = getAfbeelding(product.ProductId);
            }
            return producten;
        }

        // Get alle producten met details
        public List<ProductDetail> getProductenDetail()
        {
            List<ProductDetail> producten = new List<ProductDetail>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT detailId, verkoopprijs, inkoopprijs, maatId, voorraad, naam, omschrijving, categorieId, pd.productId FROM productdetail pd LEFT JOIN product p on pd.productId = p.productId;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        ProductDetail productDetail = getFullProductFromDataReader(dataReader);
                        producten.Add(productDetail);
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
        private string getAfbeelding(int productId)
        {
            string path = null;
            try
            {
                conn.Open();

                string selectQuery = "SELECT locatie FROM afbeelding where productId = @productId;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter idParam = new MySqlParameter("@productId", MySqlDbType.Int32);
                idParam.Value = productId;
                cmd.Parameters.Add(idParam);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        path = dataReader.GetString("locatie");
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
            return path;
        }

        // Get 1 product zonder details
        public ProductBase GetProduct(int id)
        {
            ProductBase product = null;
            try
            {
                conn.Open();

                string selectQuery = "SELECT * FROM product WHERE productId = @productId;";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter productIdParam = new MySqlParameter("@productId", MySqlDbType.Int32);

                productIdParam.Value = id;

                cmd.Parameters.Add(productIdParam);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    product = GetProductFromDataReader(dataReader);
                }

                return product;
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van product mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        // Get 1 product zonder details via detailId
        public ProductBase GetProductByDetail(int id)
        {
            ProductBase product = null;
            try
            {
                conn.Open();

                string selectQuery = "SELECT * from product WHERE productId = (SELECT pd.productId FROM productDetail pd WHERE detailId = @detailId);";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter detailIdParam = new MySqlParameter("@detailId", MySqlDbType.Int32);

                detailIdParam.Value = id;

                cmd.Parameters.Add(detailIdParam);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    product = GetProductFromDataReader(dataReader);
                }

                return product;
            }
            catch (Exception e)
            {
                Console.Write("Ophalen van product mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        // Get 1 product met details
        public ProductDetail GetProductAndDetail(int id)
        {
            ProductDetail productDetail = null;
            try{                
            conn.Open();

            string selectQuery = "SELECT detailId, verkoopprijs, inkoopprijs, maatId, voorraad, naam, omschrijving, categorieId, pd.productId FROM productdetail pd LEFT JOIN product on pd.productId = @productId;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter productIdParam = new MySqlParameter("@productId", MySqlDbType.Int32);

                productIdParam.Value = id;

                cmd.Parameters.Add(productIdParam);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                    while(dataReader.Read())
                        productDetail = getFullProductFromDataReader(dataReader);

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

            return productDetail;
        }

        // Get 1 productdetail
        public ProductDetail GetProductDetail(int id)
        {
            ProductDetail productDetail = new ProductDetail();
            try
            {
                conn.Open();

                string selectQuery = "SELECT detailId, verkoopprijs, inkoopprijs, maatId, voorraad, naam, omschrijving, categorieId, pd.productId FROM productdetail pd LEFT JOIN product p on pd.productId = p.productId where pd.detailId = @detailId;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter detailIdParam = new MySqlParameter("@detailId", MySqlDbType.Int32);

                detailIdParam.Value = id;

                cmd.Parameters.Add(detailIdParam);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                    while (dataReader.Read())
                        productDetail = getFullProductFromDataReader(dataReader);

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

            return productDetail;
        }
        public int insertProductAndAfbeeldingForToeveogenProductDetail(ProductBase product)
        {
            InsertProduct(product);
            product.ProductId = getProductId(product);
            InsertAfbeelding(product.ProductId, product.AfbeeldingPath);
            return product.ProductId;
        }

        // Get alle maten
        public List<ProductMaat> GetMaten()
        {
            List<ProductMaat> maten = new List<ProductMaat>();
            try
            {
                conn.Open();

                string selectQuery = "SELECT maatId, maat FROM maat;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        ProductMaat maat = getFullMaatFromDataReader(dataReader);
                        maten.Add(maat);
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write("Ophalen van maten mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return maten;
        }

        // Insert Product
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

                string selectQuery = @"Update verkoopprijs, inkoopprijs, maatId, voorraad, naam, omschrijving, FROM productdetail LEFT JOIN product on productdetail.productId = product.productId;";
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
        public int getProductId(ProductBase product)
        {
            int id= 0;
            try
            {
                conn.Open();

                string selectQuery = "SELECT productId from product where omschrijving = @omschrijving and naam  = @naam";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter naamParam = new MySqlParameter("@naam", MySqlDbType.VarChar);
                MySqlParameter omschrijvingParam = new MySqlParameter("@omschrijving", MySqlDbType.VarChar);
                naamParam.Value = product.Naam;
                omschrijvingParam.Value = product.Omschrijving;

                cmd.Parameters.Add(naamParam);
                cmd.Parameters.Add(omschrijvingParam);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {

                        id = dataReader.GetInt32("productId");
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
            return id;
        }
        // Insert 1 productDetail
        public void InsertProductDetail(ProductDetail productDetail)
        {
            try
            {
                conn.Open();
                string insertString = @"insert into productdetail (verkoopprijs, inkoopprijs, maatId, voorraad, productId) " +
                                        "values (@verkoopprijs, @inkoopprijs, @maatId, @voorraad, @productId)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter verkoopprijsParam = new MySqlParameter("@verkoopprijs", MySqlDbType.Double);
                MySqlParameter inkoopprijsParam = new MySqlParameter("@inkoopprijs", MySqlDbType.Double);
                MySqlParameter maatParam = new MySqlParameter("@maatId", MySqlDbType.Int32);
                MySqlParameter voorraadParam = new MySqlParameter("@voorraad", MySqlDbType.Int32);
                MySqlParameter productIdParam = new MySqlParameter("@productId", MySqlDbType.Int32);

                verkoopprijsParam.Value = productDetail.verkoopprijs;
                inkoopprijsParam.Value = productDetail.inkoopprijs;
                maatParam.Value = productDetail.maatId;
                voorraadParam.Value = productDetail.voorraad;
                productIdParam.Value = productDetail.productId;

                cmd.Parameters.Add(verkoopprijsParam);
                cmd.Parameters.Add(inkoopprijsParam);
                cmd.Parameters.Add(maatParam);
                cmd.Parameters.Add(voorraadParam);
                cmd.Parameters.Add(productIdParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("ProductDetail niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        public void InsertAfbeelding(int productId, string locatie)
        {
            try
            {
                conn.Open();
                string insertString = @"insert into afbeelding (locatie, productid) " +
                                        "values (@locatie, @productid)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter locatieParam = new MySqlParameter("@locatie", MySqlDbType.VarChar);
                MySqlParameter productIdParam = new MySqlParameter("@productid", MySqlDbType.Int32);

                locatieParam.Value = locatie;
                productIdParam.Value = productId;

                cmd.Parameters.Add(locatieParam);
                cmd.Parameters.Add(productIdParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("ProductDetail niet toegevoegd: " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        // Update 1 productdetail
        public void UpdateProductDetail(ProductDetail productDetail)
        {
            try
            {
                conn.Open();

                string updateQuery = @"UPDATE productdetail SET verkoopprijs = @verkoopprijs, inkoopprijs = @inkoopprijs, maatId = @maatId, 
                                        voorraad = @voorraad WHERE detailId = @detailId;";
                MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                MySqlParameter verkoopprijsParam = new MySqlParameter("@verkoopprijs", MySqlDbType.Double);
                MySqlParameter inkoopprijsParam = new MySqlParameter("@inkoopprijs", MySqlDbType.Double);
                MySqlParameter maatParam = new MySqlParameter("@maatId", MySqlDbType.Int32);
                MySqlParameter voorraadParam = new MySqlParameter("@voorraad", MySqlDbType.Int32);
                MySqlParameter detailIdParam = new MySqlParameter("@detailId", MySqlDbType.Int32);

                verkoopprijsParam.Value = productDetail.verkoopprijs;
                inkoopprijsParam.Value = productDetail.inkoopprijs;
                maatParam.Value = productDetail.maatId;
                voorraadParam.Value = productDetail.voorraad;
                detailIdParam.Value = productDetail.detailId;

                cmd.Parameters.Add(verkoopprijsParam);
                cmd.Parameters.Add(inkoopprijsParam);
                cmd.Parameters.Add(maatParam);
                cmd.Parameters.Add(voorraadParam);
                cmd.Parameters.Add(detailIdParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.Write("ProductDetail update mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void VerwijderProductDetail(int id)
        {
            try
            {
                conn.Open();

                string insertString = @"delete from productdetail where detailId = @detailId";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter detailidParam = new MySqlParameter("@detailId", MySqlDbType.Int32);

                detailidParam.Value = id;

                cmd.Parameters.Add(detailidParam);

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

                string selectQuery = "SELECT productId, naam, omschrijving, categorieId from product where categorieId = @categorieId";
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

                string selectQuery = "SELECT productId, naam, omschrijving, categorieId from product where naam like @search";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter naamParam = new MySqlParameter("@search", MySqlDbType.VarChar);
                naamParam.Value = search;
                cmd.Parameters.Add(naamParam);
                cmd.Prepare();
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