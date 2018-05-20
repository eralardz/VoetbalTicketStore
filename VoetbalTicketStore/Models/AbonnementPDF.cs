using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoetbalTicketStore.Models
{
    public class AbonnementPDF
    {
        public int AbonnementId { get; set; }
        public int BestellingId { get; set; }
        public decimal Prijs { get; set; }
        public string ClubNaam { get; set; }
        public string StadionNaam { get; set; }
        public int SeizoenJaar { get; set; }
        public string BezoekerVoornaam { get; set; }
        public string BezoekerNaam { get; set; }
        public string BezoekerRijksregisternummer { get; set; }
        public string BezoekerEmail { get; set; }
    }
}