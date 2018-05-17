using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.ViewModel
{
    public class AbonnementBuy
    {
        public int PloegId { get; set; }
        public string PloegNaam { get; set; }
        public string Logo { get; set; }
        public int Stadionid { get; set; }
        public string StadionNaam { get; set; }
        public IEnumerable<Vak> Vakken { get; set; }
        public int GeselecteerdVakId { get; set; }
        public decimal Prijs { get; set; }
        [DisplayName("Aantal")]
        public int AantalAbonnementen { get; set; }
        public List<SelectListItem> HoeveelheidAbonnementenList { get; set; }

    }
}