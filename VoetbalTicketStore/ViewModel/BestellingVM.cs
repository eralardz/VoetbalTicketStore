using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.ViewModel
{
    public class BestellingVM
    {
        public IEnumerable<Bestelling> Bestellingen { get; set; }

        public int TicketId { get; set; }
        public int BestellingId { get; set; }
        public decimal Prijs { get; set; }
        public string ThuisploegNaam { get; set; }
        public string TegenstandersNaam { get; set; }
        public string StadionAdres { get; set; }
        public string StadionNaam { get; set; }
        public string BezoekerNaam { get; set; }
        public string BezoekerVoornaam{ get; set; }
        public string BezoekerRijksregisternummer { get; set; }
        public string BezoekerEmail { get; set; }
        public DateTime WedstrijdDatumEnTijd { get; set; }
    }
}