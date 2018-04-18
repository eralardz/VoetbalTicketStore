using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.ViewModel
{
    public class TicketWedstrijd
    {
        public Wedstrijd wedstrijd { get; set; }
        public Ticket ticket { get; set; }
        public Bezoeker bezoeker { get; set; }
    }
}