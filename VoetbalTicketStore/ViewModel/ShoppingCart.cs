using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.ViewModel
{
    public class ShoppingCart
    {
        public Bestelling bestelling { get; set; }
        public decimal totaalPrijs { get; set; }
    }
}