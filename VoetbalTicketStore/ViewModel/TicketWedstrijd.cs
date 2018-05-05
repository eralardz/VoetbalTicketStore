using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;

// ViewModel om het kopen van een Ticket voor een Wedstrijd af te handelen
namespace VoetbalTicketStore.ViewModel
{
    public class TicketWedstrijd
    {
        public Wedstrijd Wedstrijd { get; set; }
        public Ticket Ticket { get; set; }
        public Bezoeker Bezoeker { get; set; }
        public Stadion Stadion { get; set; }

        public Club Thuisploeg { get; set; }
        public Club Tegenstanders { get; set; }

        // Vakken en vaktypes
        public SelectList Vakken { get; set; }
        public IEnumerable<Vak> VakkenList { get; set; }
        public int SelectedVak { get; set; }
    }
}