using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.ViewModel
{
    public class ShoppingCart
    {
        public Bestelling Bestelling{ get; set; }
        public IList<ShoppingCartData> ShoppingCartEntries { get; set; }
        public decimal TotaalPrijs { get; set; }
    }
}