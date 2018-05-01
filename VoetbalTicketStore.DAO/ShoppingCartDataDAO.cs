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
        public void AddShoppingCartData(ShoppingCartData shoppingCartData)
        {
            using(var db = new VoetbalEntities())
            {
                db.ShoppingCartDatas.Add(shoppingCartData);
                db.SaveChanges();
            }
        }
    }
}
