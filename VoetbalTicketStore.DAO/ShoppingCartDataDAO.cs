using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO
{
    public class ShoppingCartDataDAO
    {
        public void AddToShoppingCart(ShoppingCartData shoppingCartData)
        {
            using (var db = new VoetbalstoreEntities())
            {
                db.ShoppingCartDatas.Add(shoppingCartData);
                db.SaveChanges();
            }
        }
    }
}
