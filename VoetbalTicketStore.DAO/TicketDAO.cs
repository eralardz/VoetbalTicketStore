using System;
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
            var db = new VoetbalstoreEntities();

            // lazy...
            // TODO herschrijven naar eager, echter niet eenvoudig wegens gedrag include (moet VOOR group by volgens compiler, maar NA group by volgens documentatie)
            return db.Tickets.Where(t => t.Gebruikerid.Equals(user) && t.Bezoekerrijksregisternummer == null && t.Wedstrijd.DatumEnTijd >= vanaf).GroupBy(b => b.Bestelling).ToList();
        }

        public Ticket FindTicket(int teWijzigenTicket)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Tickets.Where(t => t.Id == teWijzigenTicket).Include(t => t.Wedstrijd).Include(t => t.Wedstrijd.Club).Include(t => t.Wedstrijd.Club1).Include(t => t.Wedstrijd.Stadion).Include(t => t.Bezoeker).FirstOrDefault();
            }
        }

        public int GetAantalTicketsVoorAndereWedstrijdOpDezelfdeDatum(string user, int wedstrijdId, DateTime datumEnTijd)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Tickets.Where(t => t.Gebruikerid.Equals(user) && t.Wedstrijdid != wedstrijdId && DbFunctions.TruncateTime(t.Wedstrijd.DatumEnTijd) == DbFunctions.TruncateTime(datumEnTijd)).Count();
            }
        }

        // attach: https://msdn.microsoft.com/en-us/library/jj592676(v=vs.113).aspx
        public void KoppelBezoekerAanTicket(int teWijzigenTicket, string rijksregisternummer)
        {
            using (var db = new VoetbalstoreEntities())
            {
                Ticket ticket = new Ticket { Id = teWijzigenTicket, Bezoekerrijksregisternummer = rijksregisternummer };
                db.Tickets.Attach(ticket);
                var entry = db.Entry(ticket);
                entry.Property(e => e.Bezoekerrijksregisternummer).IsModified = true;
                db.SaveChanges();
            }
        }

        public IList<Ticket> GetNietGekoppeldeTicketsList(string user)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Tickets.Where(t => t.Gebruikerid.Equals(user) && t.Bezoekerrijksregisternummer == null).Include(w => w.Wedstrijd).Include(c => c.Wedstrijd.Club).Include(c => c.Wedstrijd.Club1).ToList();
            }
        }
    }
}