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
        public int Id { get; set; }
        public int BestellingId { get; set; }
        public decimal Prijs { get; set; }
        public Nullable<int> WedstrijdId { get; set; }
        public int Hoeveelheid { get; set; }
        public Nullable<int> VakId { get; set; }
        public int Thuisploeg { get; set; }
        public Nullable<int> Bezoekers { get; set; }
        public int ShoppingCartDataTypeId { get; set; }
    
        public virtual Bestelling Bestelling { get; set; }
        public virtual Club Club { get; set; }
        public virtual Club Club1 { get; set; }
        public virtual Vak Vak { get; set; }
        public virtual ShoppingCartDataType ShoppingCartDataType { get; set; }
        public virtual Wedstrijd Wedstrijd { get; set; }
    }
}
