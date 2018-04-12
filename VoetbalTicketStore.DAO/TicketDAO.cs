using System;
using System.Collections.Generic;
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

        public void AddTicket(Ticket ticket)
        {
            var db = new VoetbalEntities();
            db.Tickets.Add(ticket);
        }
    }
}
