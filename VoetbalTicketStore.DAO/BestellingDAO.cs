﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;
using System.Data.Entity;

namespace VoetbalTicketStore.DAO
{
    public class BestellingDAO
    {
        public Bestelling FindOpenstaandeBestelling(string user)
        {
            using (var db = new VoetbalstoreEntities())
            {
                // geeft null terug indien geen bestelling gevonden werd
                return db.Bestellings.Where(b => b.Bevestigd == false && b.AspNetUsersId.Equals(user)).FirstOrDefault();
            }
        }

        public Bestelling AddBestelling(Bestelling bestelling)
        {
            using (var db = new VoetbalstoreEntities())
            {
                db.Bestellings.Add(bestelling);
                db.SaveChanges();
                return bestelling;
            }
        }

        public Bestelling FindOpenstaandeBestellingDoorUser(string user)
        {
            using (var db = new VoetbalstoreEntities())
            {
                //ZONDER club-ids in de shoppingcartdata: return db.Bestellings.Where(b => b.Bevestigd == false && b.AspNetUsersId.Equals(user)).Include(s => s.ShoppingCartDatas.Select(w => w.Wedstrijd).Select(c => c.Club)).Include(s => s.ShoppingCartDatas.Select(w => w.Wedstrijd).Select(c => c.Club1)).FirstOrDefault();

                // MET club-ids in de shoppingcartdata
                // Reverse include! 
                // Include chaining!
                return db.Bestellings.Where(b => b.Bevestigd == false && b.AspNetUsersId.Equals(user)).Include(x => x.ShoppingCartDatas.Select(y => y.Club)).Include(z => z.ShoppingCartDatas.Select(r => r.Club1)).Include(x => x.ShoppingCartDatas.Select(v => v.Vak).Select(t => t.VakType)).Include(x => x.ShoppingCartDatas.Select(w => w.Wedstrijd)).Include(s => s.ShoppingCartDatas.Select(t => t.ShoppingCartDataType)).FirstOrDefault();
            }
        }

        public IEnumerable<Bestelling> All(string user)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Bestellings.Where(b => b.AspNetUsersId.Equals(user)).Include(b => b.Abonnements).Include(b => b.Tickets).Include(b => b.Tickets.Select(t => t.Wedstrijd).Select(c => c.Club).Select(c => c.Stadion)).Include(b => b.Tickets.Select(t => t.Wedstrijd).Select(c => c.Club1)).Include(b => b.Abonnements.Select(a => a.Club.Stadion)).Include(b => b.Tickets.Select(t => t.Bezoeker)).Include(b => b.Abonnements.Select(a => a.Bezoeker)).ToList();
            }
        }

        public void RemoveBestelling(string user)
        {
            using (var db = new VoetbalstoreEntities())
            {
                // cascade delete
                var toRemove = db.Bestellings.Where(a => a.AspNetUsersId.Equals(user) && a.Bevestigd == false);
                db.Bestellings.RemoveRange(toRemove);
                db.SaveChanges();
            }
        }

        public void BevestigBestelling(Bestelling bestelling)
        {
            using (var db = new VoetbalstoreEntities())
            {
                db.Bestellings.Attach(bestelling);
                var entry = db.Entry(bestelling);
                entry.Property(e => e.Bevestigd).IsModified = true;
                db.SaveChanges();
            }
        }
    }
}
