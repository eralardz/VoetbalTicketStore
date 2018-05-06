using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class ShoppingCartDataService
    {
        private ShoppingCartDataDAO shoppingCartDataDAO;

        public ShoppingCartDataService()
        {
            shoppingCartDataDAO = new ShoppingCartDataDAO();
        }

        public void AddToShoppingCart(int bestellingId, decimal prijs, int wedstrijdId, int aantalTickets, int vakId)
        {
            // 

            // ShoppingCartData (bestellijn) aanmaken
            ShoppingCartData shoppingCartData = new ShoppingCartData()
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
}
