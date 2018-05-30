using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service.Tests;

namespace VoetbalTicketStore.DAO
{
    public class BezoekerDAO : IBezoekerDAO
    {
        public Bezoeker FindBezoeker(string rijksregisternummer)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Bezoekers.Find(rijksregisternummer);
            }
        }

        public void AddBezoeker(Bezoeker bezoeker)
        {
            using (var db = new VoetbalstoreEntities())
            {
                db.Bezoekers.Add(bezoeker);
                db.SaveChanges();
            }
        }

        public void Wijzigbezoeker(Bezoeker bezoeker)
        {
            if (bezoeker != null)
            {
                using (var db = new VoetbalstoreEntities())
                {
                    db.Bezoekers.Attach(bezoeker);
                    var entry = db.Entry(bezoeker);
                    entry.Property(e => e.Naam).IsModified = true;
                    entry.Property(e => e.Voornaam).IsModified = true;
                    entry.Property(e => e.Email).IsModified = true;
                    db.SaveChanges();
                }
            }
        }

        public void RemoveBezoeker(string rijksregisternummer)
        {
            if (rijksregisternummer != null)
            {
                using (var db = new VoetbalstoreEntities())
                {
                    Bezoeker toRemove = new Bezoeker { Rijksregisternummer = rijksregisternummer };
                    db.Entry(toRemove).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }
    }
}
