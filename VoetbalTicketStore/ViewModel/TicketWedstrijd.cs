using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.ViewModel
{
    public class TicketWedstrijd
    {
        public Wedstrijd Wedstrijd { get; set; }
        public Ticket Ticket { get; set; }
        public Bezoeker Bezoeker { get; set; }

        // Vakken en vaktypes
        public SelectList Vakken { get; set; }
        public int SelectedVak { get; set; }
    }
}