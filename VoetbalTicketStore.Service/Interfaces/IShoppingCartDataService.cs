﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public interface IShoppingCartDataService
    {
        void AddToShoppingCart(int bestellingId, decimal prijs, int wedstrijdId, int thuisploegId, int bezoekersId, int aantalTickets, int vakId, string user);
        void AddAbonnementToShoppingCart(int bestellingId, decimal prijs, int aantalAbonnementen, int vakId, int ploegId, string user);
        void IncrementAmount(ShoppingCartData shoppingCartData);
        void RemoveShoppingCartData(int id);
        void AdjustAmount(int id, int newAmount, string user, int wedstrijdId);
        void RemoveShoppingCartDataVanBestelling(int geselecteerdeBestelling);
        void RemoveAllShoppingCartData(string user);
    }
}