using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Exceptions;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Models.Constants;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.Tests.Service
{
    [TestFixture]
    public class ShoppingCartDataServiceTest
    {
        private BestellingService bestellingService;

        [SetUp]
        public void Setup()
        {
            bestellingService = new BestellingService();
        }

        [Test]
        public void AddToShoppingCartTestParametersNull()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // act + assert
            Assert.Throws<BestelException>(() => shoppingCartDataService.AddToShoppingCart(-1, -1, -1, -1, -1, -1, -1, null), Constants.ParameterNull);
        }

        [Test]
        public void AddToShoppingCartTicketVoorWedstrijdVroegerDanToegelatenTest()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);

            // act + assert
            Assert.Throws<BestelException>(() => shoppingCartDataService.AddToShoppingCart(bestelling.Id, 1337, 2, 5, 6, 2, 23, user), Constants.VroegerDanEenMaand);
        }

        [Test]
        public void AddToShoppingCartTestNewItem()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);

            try
            {
                decimal prijs = 55;
                int wedstrijdId = 11;
                int thuisploegId = 4;
                int bezoekersId = 1;
                int aantalTickets = 1;
                int vakId = 22;

                // act
                shoppingCartDataService.AddToShoppingCart(bestelling.Id, prijs, wedstrijdId, thuisploegId, bezoekersId, aantalTickets, vakId, user);

                // assert
                List<ShoppingCartData> shoppingCartDatas = bestellingService.FindOpenstaandeBestellingDoorUser(user).ShoppingCartDatas.ToList();
                Assert.AreEqual(1, shoppingCartDatas.Count());

                Assert.AreEqual(bestelling.Id, shoppingCartDatas.First().BestellingId);
                Assert.AreEqual(prijs, shoppingCartDatas.First().Prijs);
                Assert.AreEqual(wedstrijdId, shoppingCartDatas.First().WedstrijdId);
                Assert.AreEqual(thuisploegId, shoppingCartDatas.First().Thuisploeg);
                Assert.AreEqual(aantalTickets, shoppingCartDatas.First().Hoeveelheid);
                Assert.AreEqual(vakId, shoppingCartDatas.First().VakId);
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void AddToShoppingCartTestExisting()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);
            try
            {
                decimal prijs = 55;
                int wedstrijdId = 11;
                int thuisploegId = 4;
                int bezoekersId = 1;
                int aantalTickets = 1;
                int vakId = 22;

                // act - eerste keer toevoegen
                shoppingCartDataService.AddToShoppingCart(bestelling.Id, prijs, wedstrijdId, thuisploegId, bezoekersId, aantalTickets, vakId, user);

                // act - nogmaals toevoegen, gebruiker koopt nu dus twee tickets
                shoppingCartDataService.AddToShoppingCart(bestelling.Id, prijs, wedstrijdId, thuisploegId, bezoekersId, aantalTickets, vakId, user);

                // assert
                List<ShoppingCartData> shoppingCartDatas = bestellingService.FindOpenstaandeBestellingDoorUser(user).ShoppingCartDatas.ToList();
                Assert.AreEqual(1, shoppingCartDatas.Count());

                Assert.AreEqual(bestelling.Id, shoppingCartDatas.First().BestellingId);
                Assert.AreEqual(prijs, shoppingCartDatas.First().Prijs);
                Assert.AreEqual(wedstrijdId, shoppingCartDatas.First().WedstrijdId);
                Assert.AreEqual(thuisploegId, shoppingCartDatas.First().Thuisploeg);
                Assert.AreEqual(2, shoppingCartDatas.First().Hoeveelheid);
                Assert.AreEqual(vakId, shoppingCartDatas.First().VakId);
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void AddAbonnementToShoppingCartTestNew()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);
            try
            {
                decimal prijs = 55;
                int ploegId = 4;
                int aantalAbonnementen = 1;
                int vakId = 22;

                // act - toevoegen abonnement
                shoppingCartDataService.AddAbonnementToShoppingCart(bestelling.Id, prijs, aantalAbonnementen, vakId, ploegId, user);


                // assert
                List<ShoppingCartData> shoppingCartDatas = bestellingService.FindOpenstaandeBestellingDoorUser(user).ShoppingCartDatas.ToList();
                Assert.AreEqual(1, shoppingCartDatas.Count());

                Assert.AreEqual(bestelling.Id, shoppingCartDatas.First().BestellingId);
                Assert.AreEqual(prijs, shoppingCartDatas.First().Prijs);
                Assert.AreEqual(vakId, shoppingCartDatas.First().VakId);
                Assert.AreEqual(ploegId, shoppingCartDatas.First().Thuisploeg);
                Assert.AreEqual(aantalAbonnementen, shoppingCartDatas.First().Hoeveelheid);
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void AddAbonnementToShoppingCartTestExisting()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);
            try
            {
                decimal prijs = 55;
                int ploegId = 4;
                int aantalAbonnementen = 1;
                int vakId = 22;

                // act - toevoegen abonnement
                shoppingCartDataService.AddAbonnementToShoppingCart(bestelling.Id, prijs, aantalAbonnementen, vakId, ploegId, user);

                // act - nogmaals toevoegen abonnement
                shoppingCartDataService.AddAbonnementToShoppingCart(bestelling.Id, prijs, aantalAbonnementen, vakId, ploegId, user);

                // assert
                List<ShoppingCartData> shoppingCartDatas = bestellingService.FindOpenstaandeBestellingDoorUser(user).ShoppingCartDatas.ToList();
                Assert.AreEqual(1, shoppingCartDatas.Count());

                Assert.AreEqual(bestelling.Id, shoppingCartDatas.First().BestellingId);
                Assert.AreEqual(prijs, shoppingCartDatas.First().Prijs);
                Assert.AreEqual(vakId, shoppingCartDatas.First().VakId);
                Assert.AreEqual(ploegId, shoppingCartDatas.First().Thuisploeg);
                Assert.AreEqual(2, shoppingCartDatas.First().Hoeveelheid);
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void RemoveShoppingCartDataTest()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);
            try
            {
                decimal prijs = 55;
                int ploegId = 4;
                int aantalAbonnementen = 1;
                int vakId = 22;
                ShoppingCartData shoppingCartData = shoppingCartDataService.AddAbonnementToShoppingCart(bestelling.Id, prijs, aantalAbonnementen, vakId, ploegId, user);

                // act - verwijderen abonnement
                shoppingCartDataService.RemoveShoppingCartData(shoppingCartData.Id);


                // assert
                List<ShoppingCartData> shoppingCartDatas = bestellingService.FindOpenstaandeBestellingDoorUser(user).ShoppingCartDatas.ToList();
                Assert.AreEqual(0, shoppingCartDatas.Count());
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void AdjustAmountShoppingCartDataTest()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);
            try
            {
                decimal prijs = 55;
                int ploegId = 4;
                int aantalAbonnementen = 1;
                int vakId = 22;
                ShoppingCartData shoppingCartData = shoppingCartDataService.AddAbonnementToShoppingCart(bestelling.Id, prijs, aantalAbonnementen, vakId, ploegId, user);

                int nieuweHoeveelheid = 3;
                // act - aanpassen hoeveelheid
                shoppingCartDataService.AdjustAmount(shoppingCartData.Id, nieuweHoeveelheid);


                // assert
                List<ShoppingCartData> shoppingCartDatas = bestellingService.FindOpenstaandeBestellingDoorUser(user).ShoppingCartDatas.ToList();
                Assert.AreEqual(nieuweHoeveelheid, shoppingCartDatas.First().Hoeveelheid);
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void AdjustAmountShoppingCartDataToZeroTest()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);
            try
            {
                decimal prijs = 55;
                int ploegId = 4;
                int aantalAbonnementen = 1;
                int vakId = 22;
                ShoppingCartData shoppingCartData = shoppingCartDataService.AddAbonnementToShoppingCart(bestelling.Id, prijs, aantalAbonnementen, vakId, ploegId, user);

                int nieuweHoeveelheid = 0;
                // act - aanpassen hoeveelheid
                shoppingCartDataService.AdjustAmount(shoppingCartData.Id, nieuweHoeveelheid);


                // assert
                List<ShoppingCartData> shoppingCartDatas = bestellingService.FindOpenstaandeBestellingDoorUser(user).ShoppingCartDatas.ToList();
                Assert.AreEqual(0, shoppingCartDatas.Count());
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void RemoveShoppingCartDataVanBestellingTest()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);
            try
            {
                decimal prijs = 55;
                int ploegId = 4;
                int aantalAbonnementen = 1;
                int vakId = 22;

                // act - abonnement en ticket toevoegen
                ShoppingCartData shoppingCartData = shoppingCartDataService.AddAbonnementToShoppingCart(bestelling.Id, prijs, aantalAbonnementen, vakId, ploegId, user);
                ShoppingCartData shoppingCartData2 = shoppingCartDataService.AddToShoppingCart(bestelling.Id, 44, 24, 1, 4, 1, 21, user);

                // assert - 2 elementen in de shoppingcart
                List<ShoppingCartData> shoppingCartDatas = bestellingService.FindOpenstaandeBestellingDoorUser(user).ShoppingCartDatas.ToList();
                Assert.AreEqual(2, shoppingCartDatas.Count());

                // act
                shoppingCartDataService.RemoveShoppingCartDataVanBestelling(bestelling.Id);

                // assert - shoppingcart leeg
                List<ShoppingCartData> shoppingCartDatas2 = bestellingService.FindOpenstaandeBestellingDoorUser(user).ShoppingCartDatas.ToList();
                Assert.AreEqual(0, shoppingCartDatas2.Count());
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void IncrementAmountTest()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);
            try
            {
                decimal prijs = 55;
                int ploegId = 4;
                int aantalAbonnementen = 1;
                int vakId = 22;

                // act - abonnement en ticket toevoegen
                ShoppingCartData shoppingCartData = shoppingCartDataService.AddAbonnementToShoppingCart(bestelling.Id, prijs, aantalAbonnementen, vakId, ploegId, user);

                // act
                shoppingCartDataService.IncrementAmount(shoppingCartData);
                // assert
                List<ShoppingCartData> shoppingCartDatas2 = bestellingService.FindOpenstaandeBestellingDoorUser(user).ShoppingCartDatas.ToList();
                Assert.AreEqual(2, shoppingCartDatas2.First().Hoeveelheid);
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }
    }
}