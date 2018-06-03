using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO.Interfaces
{
    public interface IShoppingCartDataDAO
    {
        ShoppingCartData AddToShoppingCart(ShoppingCartData shoppingCartData);
        IEnumerable<ShoppingCartData> GetShoppingCartEntries(int bestellingId, int wedstrijdId);
        ShoppingCartData GetShoppingCartEntry(int wedstrijdId, int bestellingId, int vakId);
        ShoppingCartData GetShoppingCartEntry(int id);
        ShoppingCartData GetShoppingCartAbonnementEntry(int bestellingId, int ploegId, int vakId);
        void IncrementAmount(ShoppingCartData shoppingCartData);
        void RemoveShoppingCartData(int id);
        void AdjustAmount(int id, int newAmount);
        void RemoveShoppingCartDataVanBestelling(int geselecteerdeBestelling);
    }
}
