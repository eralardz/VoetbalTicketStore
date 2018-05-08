using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Currency)]
        public decimal Prijs { get; set; }
        public int WedstrijdId { get; set; }
        public List<SelectListItem> HoeveelheidTicketsList { get; set; }
        public int AantalTickets { get; set; }
        public int ThuisploegId { get; set; }
        public int BezoekersId { get; set; }
    }
}