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

        [Test]
        public void AddToShoppingCartTestParametersNull()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // act + assert
            Assert.Throws<BestelException>(() => shoppingCartDataService.AddToShoppingCart(-1, -1, -1, -1, -1, -1, -1, null), Constants.ParameterNull);
        }


        [Test]
        public void AddToShoppingCartTestNewItem()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            BestellingService bestellingService = new BestellingService();
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
                shoppingCartDataService.RemoveAllShoppingCartData(user);
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void AddToShoppingCartTestExisting()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            BestellingService bestellingService = new BestellingService();
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
                shoppingCartDataService.RemoveAllShoppingCartData(user);
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void AddAbonnementToShoppingCartTestNew()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            BestellingService bestellingService = new BestellingService();
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
                shoppingCartDataService.RemoveAllShoppingCartData(user);
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void AddAbonnementToShoppingCartTestExisting()
        {
            // arrange
            ShoppingCartDataService shoppingCartDataService = new ShoppingCartDataService();

            // nieuwe bestelling maken
            BestellingService bestellingService = new BestellingService();
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
                shoppingCartDataService.RemoveAllShoppingCartData(user);
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void RemoveShoppingCartDataTest()
        {

        }

        [Test]
        public void RemoveAllShoppingCartDataTest()
        {

        }

        [Test]
        public void AdjustAmountShoppingCartDataTest()
        {

        }
    }
}