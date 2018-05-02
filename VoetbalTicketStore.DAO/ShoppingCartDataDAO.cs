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
            using (var db = new VoetbalEntities())
            {
                db.ShoppingCartDatas.Add(shoppingCartData);
                db.SaveChanges();
            }
        }

        public void RemoveShoppingCartData(int shoppingCartDataId)
        {
            using (var db = new VoetbalEntities())
            {
                ShoppingCartData toRemove = new ShoppingCartData { id = shoppingCartDataId };
                db.Entry(toRemove).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public ShoppingCartData FindShoppingCartData(int shoppingCartDataId)
        {
            using (var db = new VoetbalEntities())
            {
                return db.ShoppingCartDatas.Find(shoppingCartDataId);
            }
        }
    }
}
