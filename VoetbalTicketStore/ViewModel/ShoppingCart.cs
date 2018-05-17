using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.ViewModel
{
    public class ShoppingCart
    {
        public Bestelling Bestelling{ get; set; }
        public int GeselecteerdeBestelling { get; set; }
        public IList<ShoppingCartData> ShoppingCartEntries { get; set; }
        public decimal TotaalPrijs { get; set; }
        public int SelectedShoppingCartData { get; set; }
        public int NieuweHoeveelheid { get; set; }
        public int GeselecteerdeWedstrijd { get; set; }
        public List<SelectListItem> HoeveelheidList { get; set; }

    }
}