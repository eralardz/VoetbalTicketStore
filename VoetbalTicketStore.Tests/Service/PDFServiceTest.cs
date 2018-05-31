using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;
using static VoetbalTicketStore.Service.PDFService;

namespace VoetbalTicketStore.Tests.Service
{
    [TestFixture]
    public class PDFServiceTest
    {

        [Test]
        public void SetPDFInfoTicketTest()
        {
            // arrange
            PDFService pdfService = new PDFService();

            bool ticket = true;
            int id = 1;
            int bestellingId = 2;
            decimal prijs = 3;
            string thuisploegNaam = "Club Brugge";
            string tegenstandersNaam = "Anderlecht";
            string adres = "Ergensweg 10, 9400";
            string stadionNaam = "Da stadion";
            DateTime datetime = DateTime.Now;
            string bezoekerVoornaam = "Laurens";
            string bezoekerNaam = "De Wispelaere";
            string bezoekerRijksregisternummer = "90050207940";
            string bezoekerEmail = "laurens.dewispelaere@gmail.com";

            // act
            pdfService.setPDFInfo(ticket, id, bestellingId, prijs, thuisploegNaam, tegenstandersNaam, adres, stadionNaam, datetime, bezoekerVoornaam, bezoekerNaam, bezoekerRijksregisternummer, bezoekerEmail);


            // assert
            TicketPDF ticketPDF = pdfService.ticketPDF;

            Assert.IsNotNull(ticketPDF);
            Assert.AreEqual(ticketPDF.TicketId, id);
            Assert.AreEqual(ticketPDF.BestellingId, bestellingId);
            Assert.AreEqual(ticketPDF.Prijs, prijs);
            Assert.AreEqual(ticketPDF.ThuisploegNaam, thuisploegNaam);
            Assert.AreEqual(ticketPDF.TegenstandersNaam, tegenstandersNaam);
            Assert.AreEqual(ticketPDF.StadionAdres, adres);
            Assert.AreEqual(ticketPDF.WedstrijdDatumEnTijd, datetime);
            Assert.AreEqual(ticketPDF.BezoekerVoornaam, bezoekerVoornaam);
            Assert.AreEqual(ticketPDF.BezoekerNaam, bezoekerNaam);
            Assert.AreEqual(ticketPDF.BezoekerEmail, bezoekerEmail);
            Assert.AreEqual(ticketPDF.BezoekerRijksregisternummer, bezoekerRijksregisternummer);
        }

        [Test]
        public void SetPDFInfoAbonnementTest()
        {
            // arrange
            PDFService pdfService = new PDFService();

            bool ticket = false;
            int id = 1;
            int bestellingId = 2;
            decimal prijs = 3;
            string clubNaam = "Club Brugge";
            string stadionNaam = "Da stadion";
            DateTime datetime = DateTime.Now;
            string bezoekerVoornaam = "Laurens";
            string bezoekerNaam = "De Wispelaere";
            string bezoekerRijksregisternummer = "90050207940";
            string bezoekerEmail = "laurens.dewispelaere@gmail.com";

            // act
            pdfService.setPDFInfo(ticket, id, bestellingId, prijs, clubNaam, null, null, stadionNaam, datetime, bezoekerVoornaam, bezoekerNaam, bezoekerRijksregisternummer, bezoekerEmail);

            // assert
            AbonnementPDF abonnementPDF = pdfService.abonnementPDF;

            Assert.IsNotNull(abonnementPDF);
            Assert.AreEqual(abonnementPDF.AbonnementId, id);
            Assert.AreEqual(abonnementPDF.BestellingId, bestellingId);
            Assert.AreEqual(abonnementPDF.Prijs, prijs);
            Assert.AreEqual(abonnementPDF.ClubNaam, clubNaam);
            Assert.AreEqual(abonnementPDF.SeizoenJaar, datetime.Year);
            Assert.AreEqual(abonnementPDF.BezoekerVoornaam, bezoekerVoornaam);
            Assert.AreEqual(abonnementPDF.BezoekerNaam, bezoekerNaam);
            Assert.AreEqual(abonnementPDF.BezoekerEmail, bezoekerEmail);
            Assert.AreEqual(abonnementPDF.BezoekerRijksregisternummer, bezoekerRijksregisternummer);
        }
    }
}
