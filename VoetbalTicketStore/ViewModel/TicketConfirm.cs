using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VoetbalTicketStore.ViewModel
{
    public class TicketConfirm
    {
        public int VakId { get; set; }
        public string VakNaam { get; set; }
        public int AantalVrijePlaatsen { get; set; }
        public decimal Prijs { get; set; }
        public int WedstrijdId { get; set; }
        public List<SelectListItem> HoeveelheidTicketsList { get; set; }
        public int AantalTickets { get; set; }
    }
}