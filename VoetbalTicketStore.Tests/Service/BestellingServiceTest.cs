using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.Tests.Service
{
    [TestFixture]
    public class BestellingServiceTest
    {
        public void RemoveBestellingTest()
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

                // act - abonnement en ticket toevoegen
                ShoppingCartData shoppingCartData = shoppingCartDataService.AddAbonnementToShoppingCart(bestelling.Id, prijs, aantalAbonnementen, vakId, ploegId, user);
                ShoppingCartData shoppingCartData2 = shoppingCartDataService.AddToShoppingCart(bestelling.Id, 44, 24, 1, 4, 1, 21, user);

                // assert - 2 elementen in de shoppingcart
                List<ShoppingCartData> shoppingCartDatas = bestellingService.FindOpenstaandeBestellingDoorUser(user).ShoppingCartDatas.ToList();
                Assert.AreEqual(2, shoppingCartDatas.Count());

                // act - alles abonnement
                bestellingService.RemoveBestelling(user);

                // assert - shoppingcart leeg
                List<ShoppingCartData> shoppingCartDatas2 = bestellingService.FindOpenstaandeBestellingDoorUser(user).ShoppingCartDatas.ToList();
                Assert.AreEqual(0, shoppingCartDatas2.Count());
            }
            finally
            {
                bestellingService.RemoveBestelling(user);
            }
        }

    }
}
