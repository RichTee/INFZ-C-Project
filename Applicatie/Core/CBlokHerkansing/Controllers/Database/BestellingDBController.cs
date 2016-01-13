using CBlokHerkansing.Models.Bestelling;
using CBlokHerkansing.Models.Product;
using CBlokHerkansing.Models.Winkelwagen;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBlokHerkansing.Controllers.Database
{
    public class BestellingDBController : DatabaseController
    {
        public BestellingDBController() { }

        public List<BestelRegel> GetBestellingen()
        {
            List<BestelRegel> bestellingen = new List<BestelRegel>();

            try
            {
                conn.Open();
                // TODO: Haal alleen data binnen dat relevant is, niet alles.
                string selectQuery = "SELECT be.bestellingId, be.bezorgStatus, be.bezorgTijd, be.bestelDatum, ad.adresId, ad.straat, ad.postcode, ad.huisnummer, ad.huisnummertoevoeging, ad.stad, be.gebruikerId, ge.voornaam, ge.achternaam, ge.wachtwoord, ge.email, ge.telefoonnummer, ge.goldStatus, ro.rol, br.regelId, br.hoeveelheid, br.datum, pd.detailId, pd.verkoopprijs, pd.inkoopprijs, pd.maat, pd.kleur, pd.voorraad, pd.productId, aa.aanbiedingId, aa.beginDatum, aa.eindDatum, aa.kortingsPercentage FROM bestelling be JOIN adres ad on ad.adresId = be.adresId JOIN gebruiker ge on ge.gebruikerId = be.gebruikerId JOIN bestelregel br on br.bestelId = be.bestellingId JOIN productdetail pd on pd.detailId = br.detailId JOIN rol ro on ro.rolId = ge.rolId LEFT JOIN aanbieding aa on aa.aanbiedingId = br.aanbiedingId;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        BestelRegel bestelRegel = getFullBestellingFromDataReader(dataReader);
                        bestellingen.Add(bestelRegel);
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write("Ophalen van bestellingen mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return bestellingen;
        }

        public List<BestelRegel> GetBestelling(int usr)
        {
            List<BestelRegel> bestellingen = new List<BestelRegel>();

            try
            {
                conn.Open();
                // TODO: Haal alleen data binnen dat relevant is, niet alles.
                string selectQuery = "SELECT be.bestellingId, be.bezorgStatus, be.bezorgTijd, be.bestelDatum, ad.adresId, ad.straat, ad.postcode, ad.huisnummer, ad.huisnummertoevoeging, ad.stad, be.gebruikerId, ge.voornaam, ge.achternaam, ge.wachtwoord, ge.email, ge.telefoonnummer, ge.goldStatus, ro.rol, br.regelId, br.hoeveelheid, br.datum, pd.detailId, pd.verkoopprijs, pd.inkoopprijs, pd.maat, pd.kleur, pd.voorraad, pd.productId, aa.aanbiedingId, aa.beginDatum, aa.eindDatum, aa.kortingsPercentage FROM bestelling be JOIN adres ad on ad.adresId = be.adresId JOIN gebruiker ge on ge.gebruikerId = be.gebruikerId JOIN bestelregel br on br.bestelId = be.bestellingId JOIN productdetail pd on pd.detailId = br.detailId JOIN rol ro on ro.rolId = ge.rolId LEFT JOIN aanbieding aa on aa.aanbiedingId = br.aanbiedingId WHERE be.gebruikerId = @gebruikerId;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter gebruikerIdParam = new MySqlParameter("@gebruikerId", MySqlDbType.Int32);

                gebruikerIdParam.Value = usr;

                cmd.Parameters.Add(gebruikerIdParam);
                cmd.Prepare();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    BestelRegel bestelRegel = getFullBestellingFromDataReader(dataReader);
                    bestellingen.Add(bestelRegel);
                }

            }
            catch (Exception e)
            {
                Console.Write("Ophalen van bestelling mislukt " + e);
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return bestellingen;
        }

        public bool InsertBestelling(WinkelwagenItem item, string datum, int gebruikerId, int adresId, int aanbiedingId)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();

                trans = conn.BeginTransaction();

                // Insert BestelBase & Retrieve last inserted Id
                int bestellingId = InsertBestelBase(datum, gebruikerId, adresId);
                
                // Doorloop alle winkelwagen items
                for (int i = 0; i < item.product.Count; i++)
                    InsertBestelRegel(item.product[i].detailId, datum, item.hoeveelheid[i],bestellingId, aanbiedingId);
                    

                trans.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exceptie in Insert Bestelling en BestellingRegel, Transactie mislukt");
                trans.Rollback();
                return false;
            }

            return true;
        }
        
        public int InsertBestelBase(string datum, int gebruikerId, int adresId)
        {
            int id = 0;
            MySqlCommand cmd = null;
            try
            {
                // TODO: Haal alleen data binnen dat relevant is, niet alles.
                string insertQuery = @"insert into bestelling (bezorgStatus,  bezorgTijd, bestelDatum, adresId, gebruikerId) 
                                                        values (@bezorgStatus,@bezorgTijd, @bestelDatum, @adresId, @gebruikerId)";

                cmd = new MySqlCommand(insertQuery, conn);
                MySqlParameter bezorgStatusParam = new MySqlParameter("@bezorgStatus", MySqlDbType.VarChar);
                MySqlParameter bezorgTijdParam = new MySqlParameter("@bezorgTijd", MySqlDbType.VarChar);
                MySqlParameter bestelDatumParam = new MySqlParameter("@bestelDatum", MySqlDbType.VarChar);
                MySqlParameter adresIdParam = new MySqlParameter("@adresId", MySqlDbType.Int32);
                MySqlParameter gebruikerIdParam = new MySqlParameter("@gebruikerId", MySqlDbType.Int32);

                bezorgStatusParam.Value = "Pending"; // TODO: Enum
                bezorgTijdParam.Value = "3 dagen";
                bestelDatumParam.Value = datum;
                adresIdParam.Value = adresId;
                gebruikerIdParam.Value = gebruikerId;

                cmd.Parameters.Add(bezorgStatusParam);
                cmd.Parameters.Add(bezorgTijdParam);
                cmd.Parameters.Add(bestelDatumParam);
                cmd.Parameters.Add(adresIdParam);
                cmd.Parameters.Add(gebruikerIdParam);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Toevoegen van bestelling mislukt " + e);
                throw e;
            }

            return id = (int) cmd.LastInsertedId;
        }

        public void InsertBestelRegel(int detailId, string datum, int hoeveelheid, int bestellingId, int aanbiedingId)
        {
            try
            {
                // TODO: Haal alleen data binnen dat relevant is, niet alles.
                string insertQuery = @"insert into bestelregel (hoeveelheid,  datum, bestelId, detailid, aanbiedingId) 
                                                        values (@hoeveelheid, @datum, @bestelId, @detailid, @aanbiedingId)";

                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                MySqlParameter hoeveelheidParam = new MySqlParameter("@hoeveelheid", MySqlDbType.VarChar);
                MySqlParameter datumParam = new MySqlParameter("@datum", MySqlDbType.VarChar);
                MySqlParameter bestelIdParam = new MySqlParameter("@bestelId", MySqlDbType.VarChar);
                MySqlParameter detailIdParam = new MySqlParameter("@detailid", MySqlDbType.Int32);
                MySqlParameter aanbiedingIdParam = new MySqlParameter("@aanbiedingId", MySqlDbType.Int32);

                hoeveelheidParam.Value = hoeveelheid;
                datumParam.Value = datum;
                bestelIdParam.Value = bestellingId;
                detailIdParam.Value = detailId;
                aanbiedingIdParam.Value = aanbiedingId == 0 ? (int?) null : aanbiedingId;

                cmd.Parameters.Add(hoeveelheidParam);
                cmd.Parameters.Add(datumParam);
                cmd.Parameters.Add(bestelIdParam);
                cmd.Parameters.Add(detailIdParam);
                cmd.Parameters.Add(aanbiedingIdParam);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write("Toevoegen van bestelregel mislukt " + e);
                throw e;
            }
        }


    }
}