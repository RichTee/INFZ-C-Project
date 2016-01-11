CREATE DATABASE  IF NOT EXISTS `dierenzaak` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `dierenzaak`;
-- MySQL dump 10.13  Distrib 5.7.9, for Win64 (x86_64)
--
-- Host: localhost    Database: dierenzaak
-- ------------------------------------------------------
-- Server version	5.6.25-log

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
  `kortingsPercentage` int(11) DEFAULT '0',
  PRIMARY KEY (`aanbiedingId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aanbieding`
--

LOCK TABLES `aanbieding` WRITE;
/*!40000 ALTER TABLE `aanbieding` DISABLE KEYS */;
INSERT INTO `aanbieding` VALUES (1,'2015-12-05','2015-12-12',15);
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
  `gebruikerId` int(11) NOT NULL,
  PRIMARY KEY (`adresId`),
  KEY `FK_KlantAdres_idx` (`gebruikerId`),
  CONSTRAINT `FK_KlantAdres` FOREIGN KEY (`gebruikerId`) REFERENCES `gebruiker` (`gebruikerId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `adres`
--

LOCK TABLES `adres` WRITE;
/*!40000 ALTER TABLE `adres` DISABLE KEYS */;
INSERT INTO `adres` VALUES (1,'teststraat','testcode','8',NULL,'world',1);
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
  `productId` int(11) NOT NULL,
  PRIMARY KEY (`afbeeldingId`),
  KEY `FK_ProductAfbeelding_idx` (`productId`),
  CONSTRAINT `FK_ProductAfbeelding` FOREIGN KEY (`productId`) REFERENCES `product` (`productId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `afbeelding`
--

LOCK TABLES `afbeelding` WRITE;
/*!40000 ALTER TABLE `afbeelding` DISABLE KEYS */;
INSERT INTO `afbeelding` VALUES (1,'voedsel afbeelding','locatie','voedsel',1);
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
  `gebruikerId` int(11) NOT NULL,
  PRIMARY KEY (`bestellingId`),
  KEY `FK_BestellingAdres_idx` (`adresId`),
  KEY `FK_BestellingGebruiker_idx` (`gebruikerId`),
  CONSTRAINT `FK_BestellingAdres` FOREIGN KEY (`adresId`) REFERENCES `adres` (`adresId`),
  CONSTRAINT `FK_BestellingGebruiker` FOREIGN KEY (`gebruikerId`) REFERENCES `gebruiker` (`gebruikerId`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bestelling`
--

LOCK TABLES `bestelling` WRITE;
/*!40000 ALTER TABLE `bestelling` DISABLE KEYS */;
INSERT INTO `bestelling` VALUES (1,'pending','3 dagen','iets',1,1),(2,'pending','3 dagen','iets',1,1);
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
  `bestelId` int(11) DEFAULT NULL,
  `detailId` int(11) NOT NULL,
  `aanbiedingId` int(11) DEFAULT NULL,
  PRIMARY KEY (`regelId`),
  KEY `FK_RegelBestelling_idx` (`bestelId`),
  KEY `FK_RegelDetail_idx` (`detailId`),
  KEY `FK_AanbiedingDetail_idx` (`aanbiedingId`),
  CONSTRAINT `FK_AanbiedingDetail` FOREIGN KEY (`aanbiedingId`) REFERENCES `aanbieding` (`aanbiedingId`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `FK_RegelBestelling` FOREIGN KEY (`bestelId`) REFERENCES `bestelling` (`bestellingId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_RegelDetail` FOREIGN KEY (`detailId`) REFERENCES `productdetail` (`detailId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bestelregel`
--

LOCK TABLES `bestelregel` WRITE;
/*!40000 ALTER TABLE `bestelregel` DISABLE KEYS */;
INSERT INTO `bestelregel` VALUES (1,20,'2016-12-13',NULL,1,NULL);
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
  `hoofdId` int(11) DEFAULT NULL,
  PRIMARY KEY (`categorieId`),
  KEY `SubID` (`hoofdId`),
  CONSTRAINT `FK_SubCatergorie` FOREIGN KEY (`hoofdId`) REFERENCES `categorie` (`categorieId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categorie`
--

LOCK TABLES `categorie` WRITE;
/*!40000 ALTER TABLE `categorie` DISABLE KEYS */;
INSERT INTO `categorie` VALUES (1,'honden','hoden catgorie',NULL);
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
  `voornaam` varchar(45) DEFAULT NULL,
  `achternaam` varchar(45) DEFAULT NULL,
  `wachtwoord` varchar(45) NOT NULL,
  `email` varchar(45) DEFAULT NULL,
  `telefoonnummer` varchar(45) DEFAULT NULL,
  `goldStatus` varchar(45) DEFAULT NULL,
  `rolId` int(11) DEFAULT '2',
  PRIMARY KEY (`gebruikerId`),
  KEY `FK_GebruikerRol_idx` (`rolId`),
  CONSTRAINT `FK_GebruikerRol` FOREIGN KEY (`rolId`) REFERENCES `rol` (`rolId`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gebruiker`
--

LOCK TABLES `gebruiker` WRITE;
/*!40000 ALTER TABLE `gebruiker` DISABLE KEYS */;
INSERT INTO `gebruiker` VALUES (1,'bram','test','test','bram@test.nl','test','test',1),(2,'manager','manager','test','manager@test.nl','test','test',3),(3,'klant','klant','test','klant@test.nl','test','test',2);
/*!40000 ALTER TABLE `gebruiker` ENABLE KEYS */;
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
  `categorieId` int(11) NOT NULL,
  PRIMARY KEY (`productId`),
  CONSTRAINT `FK_ProductCategorie` FOREIGN KEY (`productId`) REFERENCES `categorie` (`categorieId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product`
--

LOCK TABLES `product` WRITE;
/*!40000 ALTER TABLE `product` DISABLE KEYS */;
INSERT INTO `product` VALUES (1,'honden voedsel','voer voor de hond',1);
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
  `productId` int(11) NOT NULL,
  PRIMARY KEY (`detailId`),
  KEY `FK_DetailProduct_idx` (`productId`),
  CONSTRAINT `FK_DetailProduct` FOREIGN KEY (`productId`) REFERENCES `product` (`productId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productdetail`
--

LOCK TABLES `productdetail` WRITE;
/*!40000 ALTER TABLE `productdetail` DISABLE KEYS */;
INSERT INTO `productdetail` VALUES (1,20,10,2,'rood',20,1);
/*!40000 ALTER TABLE `productdetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rol`
--

DROP TABLE IF EXISTS `rol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rol` (
  `rolId` int(11) NOT NULL,
  `rol` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`rolId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rol`
--

LOCK TABLES `rol` WRITE;
/*!40000 ALTER TABLE `rol` DISABLE KEYS */;
INSERT INTO `rol` VALUES (1,'admin'),(2,'klant'),(3,'manager');
/*!40000 ALTER TABLE `rol` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-01-11 10:44:08
