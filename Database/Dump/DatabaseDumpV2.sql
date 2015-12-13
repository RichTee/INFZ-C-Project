CREATE DATABASE  IF NOT EXISTS `dierenzaak` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `dierenzaak`;
-- MySQL dump 10.13  Distrib 5.6.24, for Win64 (x86_64)
--
-- Host: localhost    Database: dierenzaak
-- ------------------------------------------------------
-- Server version	5.6.26-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `aanbieding`
--

DROP TABLE IF EXISTS `aanbieding`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aanbieding` (
  `aanbiedingId` int(11) NOT NULL AUTO_INCREMENT,
  `beginDatum` date DEFAULT NULL,
  `eindDatum` date DEFAULT NULL,
  `kortingsPercentage` int(11) DEFAULT NULL,
  `kortingsBedrag` double DEFAULT NULL,
  `regelId` int(11) DEFAULT NULL,
  PRIMARY KEY (`aanbiedingId`),
  KEY `FK_AanbiedingRegel_idx` (`regelId`),
  CONSTRAINT `FK_AanbiedingRegel` FOREIGN KEY (`regelId`) REFERENCES `bestelregel` (`regelId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aanbieding`
--

LOCK TABLES `aanbieding` WRITE;
/*!40000 ALTER TABLE `aanbieding` DISABLE KEYS */;
/*!40000 ALTER TABLE `aanbieding` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `adres`
--

DROP TABLE IF EXISTS `adres`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `adres` (
  `adresId` int(11) NOT NULL AUTO_INCREMENT,
  `straat` varchar(45) DEFAULT NULL,
  `postcode` varchar(45) NOT NULL,
  `huisnummer` varchar(45) NOT NULL,
  `huisnummertoevoeging` varchar(45) DEFAULT NULL,
  `stad` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`adresId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `adres`
--

LOCK TABLES `adres` WRITE;
/*!40000 ALTER TABLE `adres` DISABLE KEYS */;
/*!40000 ALTER TABLE `adres` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `afbeelding`
--

DROP TABLE IF EXISTS `afbeelding`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `afbeelding` (
  `afbeeldingId` int(11) NOT NULL AUTO_INCREMENT,
  `omschrijving` varchar(100) DEFAULT NULL,
  `locatie` varchar(45) DEFAULT NULL,
  `title` varchar(45) DEFAULT NULL,
  `productId` int(11) DEFAULT NULL,
  PRIMARY KEY (`afbeeldingId`),
  KEY `FK_ProductAfbeelding_idx` (`productId`),
  CONSTRAINT `FK_ProductAfbeelding` FOREIGN KEY (`productId`) REFERENCES `product` (`productId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `afbeelding`
--

LOCK TABLES `afbeelding` WRITE;
/*!40000 ALTER TABLE `afbeelding` DISABLE KEYS */;
/*!40000 ALTER TABLE `afbeelding` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bestelling`
--

DROP TABLE IF EXISTS `bestelling`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bestelling` (
  `bestellingId` int(11) NOT NULL AUTO_INCREMENT,
  `bezorgStatus` varchar(45) DEFAULT NULL,
  `bezorgTijd` varchar(45) DEFAULT NULL,
  `bestelDatum` varchar(45) DEFAULT NULL,
  `adresId` int(11) NOT NULL,
  PRIMARY KEY (`bestellingId`),
  KEY `FK_BestellingAdres_idx` (`adresId`),
  CONSTRAINT `FK_BestellingAdres` FOREIGN KEY (`adresId`) REFERENCES `adres` (`adresId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bestelling`
--

LOCK TABLES `bestelling` WRITE;
/*!40000 ALTER TABLE `bestelling` DISABLE KEYS */;
/*!40000 ALTER TABLE `bestelling` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bestelregel`
--

DROP TABLE IF EXISTS `bestelregel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bestelregel` (
  `regelId` int(11) NOT NULL AUTO_INCREMENT,
  `hoeveelheid` int(11) DEFAULT NULL,
  `datum` date DEFAULT NULL,
  `bestellingId` int(11) DEFAULT NULL,
  PRIMARY KEY (`regelId`),
  KEY `FK_RegelBestelling_idx` (`bestellingId`),
  CONSTRAINT `FK_RegelBestelling` FOREIGN KEY (`bestellingId`) REFERENCES `bestelling` (`bestellingId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bestelregel`
--

LOCK TABLES `bestelregel` WRITE;
/*!40000 ALTER TABLE `bestelregel` DISABLE KEYS */;
/*!40000 ALTER TABLE `bestelregel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categorie`
--

DROP TABLE IF EXISTS `categorie`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `categorie` (
  `categorieId` int(11) NOT NULL AUTO_INCREMENT,
  `naam` varchar(45) DEFAULT NULL,
  `omschrijving` varchar(100) DEFAULT NULL,
  `subId` int(11) DEFAULT NULL,
  PRIMARY KEY (`categorieId`),
  KEY `SubID` (`subId`),
  CONSTRAINT `FK_SubCategorie` FOREIGN KEY (`categorieId`) REFERENCES `categorie` (`subId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categorie`
--

LOCK TABLES `categorie` WRITE;
/*!40000 ALTER TABLE `categorie` DISABLE KEYS */;
/*!40000 ALTER TABLE `categorie` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gebruiker`
--

DROP TABLE IF EXISTS `gebruiker`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gebruiker` (
  `gebruikerId` int(11) NOT NULL AUTO_INCREMENT,
  `voornaam` varchar(45) NOT NULL,
  `achternaam` varchar(45) NOT NULL,
  `wachtwoord` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `bestellingId` int(11) DEFAULT NULL,
  PRIMARY KEY (`gebruikerId`),
  KEY `FK_GebruikerBestelling_idx` (`bestellingId`),
  CONSTRAINT `FK_GebruikerBestelling` FOREIGN KEY (`bestellingId`) REFERENCES `bestelling` (`bestellingId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gebruiker`
--

LOCK TABLES `gebruiker` WRITE;
/*!40000 ALTER TABLE `gebruiker` DISABLE KEYS */;
/*!40000 ALTER TABLE `gebruiker` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `klant`
--

DROP TABLE IF EXISTS `klant`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `klant` (
  `gebruikerId` int(11) NOT NULL AUTO_INCREMENT,
  `telefoonnummer` varchar(12) NOT NULL,
  `goldStatus` varchar(5) DEFAULT NULL,
  `adresId` int(11) NOT NULL,
  PRIMARY KEY (`gebruikerId`),
  KEY `FK_KlantAdres_idx` (`adresId`),
  CONSTRAINT `FK_KlantAdres` FOREIGN KEY (`adresId`) REFERENCES `adres` (`adresId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_KlantGebruiker` FOREIGN KEY (`gebruikerId`) REFERENCES `gebruiker` (`gebruikerId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `klant`
--

LOCK TABLES `klant` WRITE;
/*!40000 ALTER TABLE `klant` DISABLE KEYS */;
/*!40000 ALTER TABLE `klant` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `medewerker`
--

DROP TABLE IF EXISTS `medewerker`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `medewerker` (
  `gebruikerId` int(11) NOT NULL AUTO_INCREMENT,
  `role` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`gebruikerId`),
  CONSTRAINT `FK_MedewerkerGebruiker` FOREIGN KEY (`gebruikerId`) REFERENCES `gebruiker` (`gebruikerId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `medewerker`
--

LOCK TABLES `medewerker` WRITE;
/*!40000 ALTER TABLE `medewerker` DISABLE KEYS */;
/*!40000 ALTER TABLE `medewerker` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `product` (
  `productId` int(11) NOT NULL AUTO_INCREMENT,
  `naam` varchar(45) DEFAULT NULL,
  `omschrijving` varchar(100) DEFAULT NULL,
  `detailId` int(11) DEFAULT NULL,
  `categorieId` int(11) DEFAULT NULL,
  PRIMARY KEY (`productId`),
  KEY `FK_ProductDetail_idx` (`detailId`),
  KEY `FK_ProductCategorie_idx` (`categorieId`),
  CONSTRAINT `FK_ProductCategorie` FOREIGN KEY (`categorieId`) REFERENCES `categorie` (`categorieId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_ProductDetail` FOREIGN KEY (`detailId`) REFERENCES `productdetail` (`detailId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product`
--

LOCK TABLES `product` WRITE;
/*!40000 ALTER TABLE `product` DISABLE KEYS */;
/*!40000 ALTER TABLE `product` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `productdetail`
--

DROP TABLE IF EXISTS `productdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `productdetail` (
  `detailId` int(11) NOT NULL AUTO_INCREMENT,
  `verkoopprijs` double DEFAULT NULL,
  `inkoopprijs` double DEFAULT NULL,
  `maat` int(11) DEFAULT NULL,
  `kleur` varchar(45) DEFAULT NULL,
  `voorraad` int(11) DEFAULT NULL,
  `regelId` int(11) DEFAULT NULL,
  `aanbiedingId` int(11) DEFAULT NULL,
  PRIMARY KEY (`detailId`),
  KEY `FK_DetailRegel_idx` (`regelId`),
  KEY `FK_DetailAanbieding_idx` (`aanbiedingId`),
  CONSTRAINT `FK_DetailAanbieding` FOREIGN KEY (`aanbiedingId`) REFERENCES `aanbieding` (`aanbiedingId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_DetailRegel` FOREIGN KEY (`regelId`) REFERENCES `bestelregel` (`regelId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productdetail`
--

LOCK TABLES `productdetail` WRITE;
/*!40000 ALTER TABLE `productdetail` DISABLE KEYS */;
/*!40000 ALTER TABLE `productdetail` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-12-11 22:46:28
