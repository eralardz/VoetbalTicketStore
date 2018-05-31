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
    public class TicketControllerTest
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
        public void AnnuleerTicketTestBuitenGeldigeTermijn()
        {
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";

            try
            {
                // arrange
                // nieuwe bestelling maken

                // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
                Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);

                decimal prijs = 55;
                int wedstrijdId = 11;
                int thuisploegId = 4;
                int bezoekersId = 1;
                int aantalTickets = 1;
                int vakId = 22;

                shoppingCartDataService.AddToShoppingCart(bestelling.Id, prijs, wedstrijdId, thuisploegId, bezoekersId, aantalTickets, vakId, user);

                bestellingService.PlaatsBestelling(bestellingService.FindOpenstaandeBestellingDoorUser(user), user);

                List<Ticket> tickets = ticketService.GetNietGekoppeldeTicketsList(user).ToList();

                // act + assert
                Assert.AreEqual(1, tickets.Count());

                Assert.Throws<BestelException>(() => ticketService.AnnuleerTicket(tickets.First().Id), Constants.TicketAnnuleren);
            }
            finally
            {
                ticketService.DeleteAlleTicketsVanUser(user);
            }
        }

        [Test]
        public void AnnuleerTicketTestBinnenGeldigeTermijn()
        {
            // arrange
            // nieuwe bestelling maken
            string user = "19ce82ed-fda7-4aa5-a28d-72b4b76d6bbb";
            try
            {
                // User heeft geen bestellingen, zal sowieso moeten aangemaakt worden
                Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(user);

                decimal prijs = 55;
                int wedstrijdId = 24;
                int thuisploegId = 4;
                int bezoekersId = 1;
                int aantalTickets = 1;
                int vakId = 22;

                shoppingCartDataService.AddToShoppingCart(bestelling.Id, prijs, wedstrijdId, thuisploegId, bezoekersId, aantalTickets, vakId, user);

                bestellingService.PlaatsBestelling(bestellingService.FindOpenstaandeBestellingDoorUser(user), user);

                List<Ticket> tickets = ticketService.GetNietGekoppeldeTicketsList(user).ToList();

                // act + assert
                Assert.AreEqual(1, tickets.Count());
                ticketService.AnnuleerTicket(tickets.First().Id);

                List<Ticket> ticketsNaDelete = ticketService.GetNietGekoppeldeTicketsList(user).ToList();
                Assert.AreEqual(0, ticketsNaDelete.Count());
            }
            finally
            {
                ticketService.DeleteAlleTicketsVanUser(user);
            }
        }

    }
}
