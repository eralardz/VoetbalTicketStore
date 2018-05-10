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

        public IEnumerable<ShoppingCartData> GetShoppingCartEntries(int bestellingId, int wedstrijdId)
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

        public ShoppingCartData GetShoppingCartEntry(int id)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.ShoppingCartDatas.Find(id);
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

        // Hoeveelheid op bestellijn aanpassen naar nieuwe hoeveelheid
        public void AdjustAmount(int id, int newAmount)
        {
            using (var db = new VoetbalstoreEntities())
            {
                ShoppingCartData shoppingCartData = new ShoppingCartData { Id = id, Hoeveelheid = newAmount };
                db.ShoppingCartDatas.Attach(shoppingCartData);
                var entry = db.Entry(shoppingCartData);
                entry.Property(e => e.Hoeveelheid).IsModified = true;
                db.SaveChanges();
            }
        }

        public void RemoveAllShoppingCartData(string user)
        {
            using (var db = new VoetbalstoreEntities())
            {
            }
        }

        public void RemoveShoppingCartDataVanBestelling(int geselecteerdeBestelling)
        {
            using (var db = new VoetbalstoreEntities())
            {
                // dit zijn 2 db hits -> TODO: verbeteren met https://www.nuget.org/packages/Z.EntityFramework.Plus.EF6/ -> kan dus met 1 hit
                // "Batch Operations method allow to perform UPDATE or DELETE operation directly in the database using a LINQ Query without loading entities in the context."
                db.ShoppingCartDatas.RemoveRange(db.ShoppingCartDatas.Where(x => x.BestellingId == geselecteerdeBestelling));
                db.SaveChanges();
            }
        }
    }
}
