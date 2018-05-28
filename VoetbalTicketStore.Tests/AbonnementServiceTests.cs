using VoetbalTicketStore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using VoetbalTicketStore.Models;

// Tests moeten lopen op een testdatabank met de aangeleverde dummy data!
namespace VoetbalTicketStore.Service.Tests
{
    [TestFixture]
    public class AbonnementServiceTests
    {

        private AbonnementService abonnementService;
        public AbonnementServiceTests()
        {
            abonnementService = new AbonnementService();
        }

        [Test]
        public void GetAantalAbonnementenPerVakVoorThuisPloegTest()
        {
            // arrange
            Club club = new Club
            {
                Id = 1 // Club Brugge
            };
            Vak vak = new Vak
            {
                Id = 2 // Vak in stadion Club Brugge
            };

            // act
            int aantalAbonnementen = abonnementService.GetAantalAbonnementenPerVakVoorThuisPloeg(club, vak);

            // assert
            Assert.That(aantalAbonnementen == 2);
        }
    }
}