//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VoetbalTicketStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShoppingCartData
    {
        public int id { get; set; }
        public int BestellingId { get; set; }
        public int Ticketid { get; set; }
        public decimal prijs { get; set; }
    
        public virtual Bestelling Bestelling { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
