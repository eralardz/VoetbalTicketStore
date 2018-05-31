using VoetbalTicketStore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Exceptions;
using VoetbalTicketStore.Models.Constants;

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

        [Test]
        public void GetAantalAbonnementenPerVakVoorThuisPloegTestParametersNull()
        {
            // arrange
            AbonnementService abonnementService = new AbonnementService();

            // act + assert
            Assert.Throws<BestelException>(() => abonnementService.GetAantalAbonnementenPerVakVoorThuisPloeg(null, null), Constants.ParameterNull);
        }

        [Test]
        public void AddAbonnementenTest()
        {

            // arrange
            AbonnementService abonnementService = new AbonnementService();
            Abonnement abo1 = new Abonnement()
            {
                AspNetUsersId = "f42bf7d5-9a65-4466-b665-e10576b2939d",
                Clubid = 1,
                Prijs = 100,
                VakId = 22,
                BestellingId = 77
            };

            Abonnement abo2 = new Abonnement()
            {
                AspNetUsersId = "f42bf7d5-9a65-4466-b665-e10576b2939d",
                Clubid = 4,
                Prijs = 200,
                VakId = 22,
                BestellingId = 77

            };

            List<Abonnement> abonnements = new List<Abonnement>();
            abonnements.Add(abo1);
            abonnements.Add(abo2);

            try
            {
                List<Abonnement> abonnementsWeggechreven =
                    abonnementService.AddAbonnementen(abonnements).ToList();

                int abo1Id = abonnementsWeggechreven.ToArray()[0].Id;
                int abo2Id = abonnementsWeggechreven.ToArray()[1].Id;

                Abonnement abo1opgehaald = abonnementService.FindAbonnement(abo1Id);
                Abonnement abo2opgehaald = abonnementService.FindAbonnement(abo2Id);

                // assert
                // abonnement1
                Assert.AreEqual(abo1opgehaald.Clubid, abo1.Clubid);
                Assert.AreEqual(abo1opgehaald.AspNetUsersId, abo1.AspNetUsersId);
                Assert.AreEqual(abo1opgehaald.VakId, abo1.VakId);
                Assert.AreEqual(abo1opgehaald.Prijs, abo1.Prijs);

                // abonnement2
                Assert.AreEqual(abo2opgehaald.Clubid, abo2.Clubid);
                Assert.AreEqual(abo2opgehaald.AspNetUsersId, abo2.AspNetUsersId);
                Assert.AreEqual(abo2opgehaald.VakId, abo2.VakId);
                Assert.AreEqual(abo2opgehaald.Prijs, abo2.Prijs);
            }
            finally
            {
                abonnementService.RemoveAbonnement(abo1.Id);
                abonnementService.RemoveAbonnement(abo2.Id);
            }
        }

        [Test]
        public void GetNietGekoppeldeAbonnementenTest()
        {
            // arrange
            AbonnementService abonnementService = new AbonnementService();

            // gekoppeld abonnement
            Abonnement abo1 = new Abonnement()
            {
                AspNetUsersId = "f42bf7d5-9a65-4466-b665-e10576b2939d",
                Clubid = 1,
                Prijs = 100,
                VakId = 22,
                BestellingId = 77,
                Bezoekerrijksregisternummer = "90050207940"
            };

            // niet gekoppeld abonnement
            Abonnement abo2 = new Abonnement()
            {
                AspNetUsersId = "f42bf7d5-9a65-4466-b665-e10576b2939d",
                Clubid = 4,
                Prijs = 200,
                VakId = 22,
                BestellingId = 77
            };

            List<Abonnement> abonnements = new List<Abonnement>();
            abonnements.Add(abo1);
            abonnements.Add(abo2);

            try
            {
                List<Abonnement> abonnementsWeggechreven =
                    abonnementService.AddAbonnementen(abonnements).ToList();

                int abo1Id = abonnementsWeggechreven.ToArray()[0].Id;
                int abo2Id = abonnementsWeggechreven.ToArray()[1].Id;

                Abonnement abo1opgehaald = abonnementService.FindAbonnement(abo1Id);
                Abonnement abo2opgehaald = abonnementService.FindAbonnement(abo2Id);

                // assert
                // abonnement1
                Assert.AreEqual(abo1opgehaald.Clubid, abo1.Clubid);
                Assert.AreEqual(abo1opgehaald.AspNetUsersId, abo1.AspNetUsersId);
                Assert.AreEqual(abo1opgehaald.VakId, abo1.VakId);
                Assert.AreEqual(abo1opgehaald.Prijs, abo1.Prijs);

                // abonnement2
                Assert.AreEqual(abo2opgehaald.Clubid, abo2.Clubid);
                Assert.AreEqual(abo2opgehaald.AspNetUsersId, abo2.AspNetUsersId);
                Assert.AreEqual(abo2opgehaald.VakId, abo2.VakId);
                Assert.AreEqual(abo2opgehaald.Prijs, abo2.Prijs);
            }
            finally
            {
                abonnementService.RemoveAbonnement(abo1.Id);
                abonnementService.RemoveAbonnement(abo2.Id);
            }
        }
    }
}