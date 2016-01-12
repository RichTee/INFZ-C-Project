using CBlokHerkansing.Models.Bestelling;
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
        
        public void InsertBestelBase()
        {

        }

        public void InsertBestelRegel()
        {

        }


    }
}