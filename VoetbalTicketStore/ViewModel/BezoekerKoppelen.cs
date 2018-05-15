using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.ViewModel
{
    public class BezoekerKoppelen
    {
        //public IEnumerable<IGrouping<Bestelling, Ticket>> NietGekoppeldeTickets { get; set; }

        public IList<Ticket> NietGekoppeldeTicketsList { get; set; }

        public Bezoeker TeWijzigenBezoeker { get; set; }
        public int TeWijzigenTicket { get; set; }
    }
}