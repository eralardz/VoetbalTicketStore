using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO
{
    public class TicketDAO
    {
        public IEnumerable<Ticket> All()
        {
            var db = new VoetbalEntities();

            return db.Tickets; // lazy-loading
        }

        public Ticket AddTicket(Ticket ticket)
        {
            using (var db = new VoetbalEntities())
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return ticket;
            }
        }

        public int FindVerkochteTicketsVakPerWedstrijd(int vakId, int wedstrijdId)
        {
            using (var db = new VoetbalEntities())
            {
                return db.Tickets.Count(t => t.Vakid == vakId && t.Wedstrijdid == wedstrijdId);
            }
        }

        public int GetHoeveelheidTickets(string user, int wedstrijdId)
        {
            using (var db = new VoetbalEntities())
            {
                return db.Tickets.Count(t => t.gebruikerid.Equals(user) && t.Wedstrijdid == wedstrijdId);
            }
        }

        public void RemoveTicket(int ticketid)
        {
            using (var db = new VoetbalEntities())
            {
                Ticket toRemove = new Ticket { id = ticketid };
                db.Entry(toRemove).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
