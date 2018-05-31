using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Exceptions;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Models.Constants;

namespace VoetbalTicketStore.Service
{
    public class ShoppingCartDataService : IShoppingCartDataService
    {
        private ShoppingCartDataDAO shoppingCartDataDAO;
        private TicketService ticketService;

        public ShoppingCartDataService()
        {
            shoppingCartDataDAO = new ShoppingCartDataDAO();
        }

        public ShoppingCartData AddToShoppingCart(int bestellingId, decimal prijs, int wedstrijdId, int thuisploegId, int bezoekersId, int aantalTickets, int vakId, string user)
        {
            if(bestellingId < 0 || prijs < 0 || wedstrijdId < 0 || thuisploegId < 0 || bezoekersId < 0 || aantalTickets < 1 || vakId < 0 || user == null)
            {
                throw new BestelException(Constants.ParameterNull);
            }

            // Hoeveelheid in ShoppingCartData verhogen als exact hetzelfde toegevoegd wordt
            ShoppingCartData shoppingCartData = shoppingCartDataDAO.GetShoppingCartEntry(wedstrijdId, bestellingId, vakId);
            if (shoppingCartData != null)
            {
                IncrementAmount(shoppingCartData);
                return shoppingCartData;
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
                    ToegevoegdOp = DateTime.Now,

                    // ticket
                    ShoppingCartDataTypeId = 1

                };

                // Toevoegen aan DB
                return shoppingCartDataDAO.AddToShoppingCart(shoppingCartData);
            }
        }

        public void AddAbonnementToShoppingCart(int bestellingId, decimal prijs, int aantalAbonnementen, int vakId, int ploegId, string user)
        {
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
                    ShoppingCartDataTypeId = 2,
                    ToegevoegdOp = DateTime.Now
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
            shoppingCartDataDAO.AdjustAmount(id, newAmount);
        }

        public void RemoveShoppingCartDataVanBestelling(int geselecteerdeBestelling)
        {
            shoppingCartDataDAO.RemoveShoppingCartDataVanBestelling(geselecteerdeBestelling);
        }

        public void RemoveAllShoppingCartData(string user)
        {
            shoppingCartDataDAO.RemoveAllShoppingCartData(user);
        }
    }
}