using System;
using System.Collections.Generic;
using System.Web.Mvc;
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

        public void AddToShoppingCart(int bestellingId, decimal prijs, int wedstrijdId, int thuisploegId, int bezoekersId, int aantalTickets, int vakId, string user)
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
                        VakId = vakId,
                        Thuisploeg = thuisploegId,
                        Bezoekers = bezoekersId,

                        // Voorlopig enkel support voor aankopen tickets (id = 1) TODO: enum van maken
                        ShoppingCartDataTypeId = 1

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

        public void AddAbonnementToShoppingCart(int bestellingId, decimal prijs, int aantalAbonnementen, int vakId, int ploegId, string user)
        {
            // geen limiet op het aankopen van abonnementen (wel begrensd in UI)

            ShoppingCartData shoppingCartData = shoppingCartDataDAO.GetShoppingCartAbonnementEntry(bestellingId, ploegId, vakId);
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
                    Hoeveelheid = aantalAbonnementen,
                    VakId = vakId,
                    Thuisploeg = ploegId,
                    ShoppingCartDataTypeId = 2
                };


                // Toevoegen aan DB
                shoppingCartDataDAO.AddToShoppingCart(shoppingCartData);
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

            foreach (ShoppingCartData item in shoppingCartDatas)
            {
                totaleHoeveelheidInShoppingCart += item.Hoeveelheid;
            }

            return (totaleHoeveelheidInShoppingCart + aantalVerkochteTickets) < Constants.MaximaalAantalTicketsPerGebruikerPerWedstrijd;
        }

        public void RemoveShoppingCartData(int id)
        {
            shoppingCartDataDAO.RemoveShoppingCartData(id);
        }

        public void AdjustAmount(int id, int newAmount, string user, int wedstrijdId)
        {

            // Heeft de gebruiker al tickets gekocht voor deze wedstrijd?
            ticketService = new TicketService();
            int aantalVerkochteTickets = ticketService.GetAantalGekochteTickets(user, wedstrijdId);

            // Maximaal aantal tickets per persoon per wedstrijd
            if (aantalVerkochteTickets + newAmount > 4)
            {
                throw new BestelException("Er mogen maximaal " + Constants.MaximaalAantalTicketsPerGebruikerPerWedstrijd + " tickets per wedstrijd aangekocht worden!");
            }
            else
            {
                shoppingCartDataDAO.AdjustAmount(id, newAmount);
            }
        }

        public void RemoveShoppingCartDataVanBestelling(int geselecteerdeBestelling)
        {
            shoppingCartDataDAO.RemoveShoppingCartDataVanBestelling(geselecteerdeBestelling);
        }

        public void RemoveAllShoppingCartData(string user)
        {
            shoppingCartDataDAO.RemoveAllShoppingCartData(user);
        }

        public void IncrementShoppingCartData(int id)
        {
            throw new NotImplementedException();
        }
    }
}