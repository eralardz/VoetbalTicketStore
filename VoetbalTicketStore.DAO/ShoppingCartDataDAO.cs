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

        public IEnumerable<ShoppingCartData> GetShoppingCartEntries (int bestellingId, int wedstrijdId)
        {
            using (var db = new VoetbalstoreEntities())
            {
                // Kunnen er ook meerdere zijn!
                return db.ShoppingCartDatas.Where(s => s.BestellingId == bestellingId && s.WedstrijdId == wedstrijdId).ToList();
            }
        }

        public ShoppingCartData GetShoppingCartEntry(int wedstrijdId, int bestellingId, int vakId)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.ShoppingCartDatas.Where(s => s.WedstrijdId == wedstrijdId && s.BestellingId == bestellingId && s.VakId == vakId).FirstOrDefault();
            }
        }

        // Attach en IsModified -> Slechts 1 DB hit (tegenover 2 bij ophalen, updaten, en dan weer wegschrijven)
        public void IncrementAmount(ShoppingCartData shoppingCartData)
        {
            using (var db = new VoetbalstoreEntities())
            {
                db.ShoppingCartDatas.Attach(shoppingCartData);
                var entry = db.Entry(shoppingCartData);
                entry.Property(e => e.Hoeveelheid).IsModified = true;
                db.SaveChanges();
            }
        }

        public void RemoveShoppingCartData(int id)
        {
            using (var db = new VoetbalstoreEntities())
            {
                ShoppingCartData toRemove = new ShoppingCartData { Id = id };
                db.Entry(toRemove).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
