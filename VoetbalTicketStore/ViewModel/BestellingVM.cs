using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.ViewModel
{
    public class BestellingVM
    {
        public IEnumerable<Bestelling> Bestellingen{ get; set; }
    }
}