using System;
using System.Collections.Generic;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Exceptions;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Models.Constants;

namespace VoetbalTicketStore.Service
{
    public class ShoppingCartDataService
    {
        private ShoppingCartDataDAO shoppingCartDataDAO;
        private TicketService ticketService;

        public ShoppingCartDataService()
        {
            shoppingCartDataDAO = new ShoppingCartDataDAO();
        }

        public void AddToShoppingCart(int bestellingId, decimal prijs, int wedstrijdId, int aantalTickets, int vakId, string user)
        {
            // Er mogen maximaal 4 tickets per wedstrijd per persoon gekocht worden
            if (GebruikerMagVoorDezeWedstrijdNogTicketsToevoegen(user, wedstrijdId, bestellingId))
            {
                // Hoeveelheid in ShoppingCartData verhogen als exact hetzelfde toegevoegd wordt
                ShoppingCartData shoppingCartData = shoppingCartDataDAO.GetShoppingCartEntry(wedstrijdId, bestellingId, vakId);
                if (shoppingCartData != null)
                {
                    IncrementAmount(shoppingCartData);
                }

                else
                {
                    // Nieuwe ShoppingCartData (bestellijn) aanmaken
                    shoppingCartData = new ShoppingCartData()
                    {
                        BestellingId = bestellingId,
                        Prijs = prijs,
                        WedstrijdId = wedstrijdId,
                        Hoeveelheid = aantalTickets,
                        VakId = vakId
                    };

                    // Toevoegen aan DB
                    shoppingCartDataDAO.AddToShoppingCart(shoppingCartData);
                }
            }
            else
            {
                throw new BestelException("Er mogen maximaal " + Constants.MaximaalAantalTicketsPerGebruikerPerWedstrijd + " tickets per wedstrijd aangekocht worden!");
            }
        }

        public void IncrementAmount(ShoppingCartData shoppingCartData)
        {
            shoppingCartData.Hoeveelheid++;
            shoppingCartDataDAO.IncrementAmount(shoppingCartData);
        }


        private bool GebruikerMagVoorDezeWedstrijdNogTicketsToevoegen(string user, int wedstrijdId, int bestellingId)
        {
            // Heeft de gebruiker al tickets gekocht voor deze wedstrijd?
            ticketService = new TicketService();
            int aantalVerkochteTickets = ticketService.GetAantalGekochteTickets(user, wedstrijdId);

            // Heeft de gebruiker al dergelijke tickets toegevoegd aan zijn winkelmandje?
            IEnumerable<ShoppingCartData> shoppingCartDatas = shoppingCartDataDAO.GetShoppingCartEntries(bestellingId, wedstrijdId);
            int totaleHoeveelheidInShoppingCart = 0;


            foreach(ShoppingCartData item in shoppingCartDatas)
            {
                totaleHoeveelheidInShoppingCart += item.Hoeveelheid;
            }

            return (totaleHoeveelheidInShoppingCart + aantalVerkochteTickets) < 4;
        }

        public void RemoveShoppingCartData(int id)
        {
            shoppingCartDataDAO = new ShoppingCartDataDAO();
            shoppingCartDataDAO.RemoveShoppingCartData(id);
        }

        public void AdjustAmount(int id, int newAmount)
        {
            throw new NotImplementedException();
        }

        public void IncrementShoppingCartData(int id)
        {
            throw new NotImplementedException();
        }
    }
}