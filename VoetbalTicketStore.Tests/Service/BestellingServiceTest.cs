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
    public class BestellingServiceTest
    {
        private BestellingService bestellingService;
        private ShoppingCartDataService shoppingCartDataService;
        private TicketService ticketService;

        [SetUp]
        public void Setup()
        {
            bestellingService = new BestellingService();
            shoppingCartDataService = new ShoppingCartDataService();
            ticketService = new TicketService();

        }

        [Test]
        public void RemoveBestellingTest()
        {
            // arrange
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

                // act - alles abonnement
                bestellingService.RemoveBestelling(user);

                // assert - shoppingcart leeg
                Assert.IsNull(bestellingService.FindOpenstaandeBestellingDoorUser(user));
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void PlaatsBestellingMeerDan4TicketsTest()
        {
            // arrange
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
                int aantalTickets = 5;
                int vakId = 22;

                shoppingCartDataService.AddToShoppingCart(bestelling.Id, prijs, wedstrijdId, thuisploegId, bezoekersId, aantalTickets, vakId, user);

                // act + assert
                Assert.Throws<BestelException>(() => bestellingService.PlaatsBestelling(bestellingService.FindOpenstaandeBestellingDoorUser(user), user), "Er mogen maximaal " + Constants.MaximaalAantalTicketsPerGebruikerPerWedstrijd + " tickets per wedstrijd aangekocht worden!");
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void PlaatsBestellingMeerDan4TicketsVerspreidOver2BestellingenTest()
        {
            // arrange
            // BESTELLING 1 - nieuwe bestelling maken
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            try
            {
                Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);

                decimal prijs = 55;
                int wedstrijdId = 11;
                int thuisploegId = 4;
                int bezoekersId = 1;
                int aantalTickets = 3;
                int vakId = 22;

                shoppingCartDataService.AddToShoppingCart(bestelling.Id, prijs, wedstrijdId, thuisploegId, bezoekersId, aantalTickets, vakId, user);

                bestellingService.PlaatsBestelling(bestellingService.FindOpenstaandeBestellingDoorUser(user), user);

                // BESTELLING 2 - nieuwe bestelling maken
                Bestelling bestelling2 = bestellingService.CreateNieuweBestellingIndienNodig(user);
                shoppingCartDataService.AddToShoppingCart(bestelling2.Id, prijs, wedstrijdId, thuisploegId, bezoekersId, aantalTickets, vakId, user);

                // act + assert
                Assert.Throws<BestelException>(() => bestellingService.PlaatsBestelling(bestellingService.FindOpenstaandeBestellingDoorUser(user), user), "Er mogen maximaal " + Constants.MaximaalAantalTicketsPerGebruikerPerWedstrijd + " tickets per wedstrijd aangekocht worden!");
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void PlaatsBestellingReedsTicketsGekochtVoorAndereWedstrijdOpDezelfdeDag()
        {
            // arrange
            // nieuwe bestelling maken
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            try
            {
                // Wedstrijd 1
                Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);

                decimal prijs = 55;
                int wedstrijdId = 5;
                int thuisploegId = 4;
                int bezoekersId = 1;
                int aantalTickets = 1;
                int vakId = 22;

                shoppingCartDataService.AddToShoppingCart(bestelling.Id, prijs, wedstrijdId, thuisploegId, bezoekersId, aantalTickets, vakId, user);

                bestellingService.PlaatsBestelling(bestellingService.FindOpenstaandeBestellingDoorUser(user), user);

                // Wedstrijd 2
                int wedstrijdId2 = 6;
                int thuisploegId2 = 4;
                int bezoekersId2 = 1;

                Bestelling bestelling2 = bestellingService.CreateNieuweBestellingIndienNodig(user);
                shoppingCartDataService.AddToShoppingCart(bestelling2.Id, prijs, wedstrijdId2, thuisploegId2, bezoekersId2, aantalTickets, vakId, user);

                // act + assert
                Assert.Throws<BestelException>(() => bestellingService.PlaatsBestelling(bestellingService.FindOpenstaandeBestellingDoorUser(user), user), "Je hebt al tickets gekocht voor een andere wedstrijd op deze dag, dit is niet toegestaan!");
            }
            finally
            {
                ticketService.DeleteAlleTicketsVanUser(user);
                bestellingService.RemoveBestelling(user);
            }
        }


        [Test]
        public void PlaatsBestellingVakVolzetDoorTickets()
        {
            // Arrange
            // Vak 11 (stadion 3, vaktypeId 1) heeft slechts 2 plaatsen
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            try
            {
                // Wedstrijd 1
                Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);
                decimal prijs = 55;
                int wedstrijdId = 6;
                int thuisploegId = 5;
                int bezoekersId = 6;
                int aantalTickets = 3;
                int vakId = 11;

                shoppingCartDataService.AddToShoppingCart(bestelling.Id, prijs, wedstrijdId, thuisploegId, bezoekersId, aantalTickets, vakId, user);

                // act + assert
                Assert.Throws<BestelException>(() => bestellingService.PlaatsBestelling(bestellingService.FindOpenstaandeBestellingDoorUser(user), user), "Er zijn nog slechts 2 tickets beschikbaar voor RSC Anderlecht - Zulte Waregem op 20/06/2018 17:00:00. Verminder het aantal tickets en probeer opnieuw. Wees snel!");
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

        [Test]
        public void PlaatsBestellingVakVolzetDoorAbonnementen()
        {
            // Arrange
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
            try
            {
                // Wedstrijd 1
                Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);
                decimal prijs = 55;
                int ploegId = 5;
                int aantalAbonnementen = 1;
                int vakId = 10;

                // abonnement toevoegen
                ShoppingCartData shoppingCartData = shoppingCartDataService.AddAbonnementToShoppingCart(bestelling.Id, prijs, aantalAbonnementen, vakId, ploegId, user);

                bestellingService.PlaatsBestelling(bestellingService.FindOpenstaandeBestellingDoorUser(user), user);

                // 3 tickets kopen voor vak 10, slechts 3 zitplaatsen, waarvan nu 1 bezet door een abonnement
                Bestelling bestelling2 = bestellingService.CreateNieuweBestellingIndienNodig(user);
                int wedstrijdId = 6;
                int thuisploegId = 5;
                int bezoekersId = 6;
                int aantalTickets = 3;
                int vakId2 = 10;

                shoppingCartDataService.AddToShoppingCart(bestelling2.Id, prijs, wedstrijdId, thuisploegId, bezoekersId, aantalTickets, vakId2, user);
                // act + assert
                Assert.Throws<BestelException>(() => bestellingService.PlaatsBestelling(bestellingService.FindOpenstaandeBestellingDoorUser(user), user));
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

    }
}