﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;
using System.Data.Entity;

namespace VoetbalTicketStore.DAO
{
    public class TicketDAO
    {

        public int GetAantalVerkochteTicketsVoorVak(Vak vak, Wedstrijd wedstrijd)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Tickets.Count(t => t.Vakid == vak.Id && t.Wedstrijdid == wedstrijd.Id);
            }
        }

        public void AddTicket(Ticket ticket)
        {
            using (var db = new VoetbalstoreEntities())
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
            }
        }

        public int GetAantalGekochteTickets(string user, int wedstrijdId)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Tickets.Count(t => t.Gebruikerid.Equals(user) && t.Wedstrijdid == wedstrijdId);
            }
        }

        public void AddTickets(IList<Ticket> tickets)
        {
            using (var db = new VoetbalstoreEntities())
            {
                db.Tickets.AddRange(tickets);
                db.SaveChanges();
            }
        }

        public IEnumerable<IGrouping<Bestelling, Ticket>> GetNietGekoppeldeTickets(string user, DateTime vanaf)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Tickets.Where(t => t.Gebruikerid.Equals(user) && t.Bezoekerrijksregisternummer == null && t.Wedstrijd.DatumEnTijd >= vanaf).Include(t => t.Wedstrijd).GroupBy(b => b.Bestelling).ToList();
            }
        }
    }
}