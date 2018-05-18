using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.ViewModel
{
    public class BezoekerKoppelen
    {
        public IList<Ticket> NietGekoppeldeTicketsList { get; set; }

        public Bezoeker TeWijzigenBezoeker { get; set; }
        public int TeWijzigenTicket { get; set; }
        public int TeWijzigenAbonnement { get; set; }
        public IEnumerable<IGrouping<Bestelling, Ticket>> NietGekoppeldeTickets { get; set; }
        public IEnumerable<Abonnement> NietGekoppeldeAbonnementen { get; set; }
    }
}