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
    
    public partial class Abonnement
    {
        public int id { get; set; }
        public int Clubid { get; set; }
        public decimal prijs { get; set; }
        public string Bezoekerrijksregisternummer { get; set; }
    
        public virtual Bezoeker Bezoeker { get; set; }
        public virtual Club Club { get; set; }
    }
}
