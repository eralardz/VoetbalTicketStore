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
    
    public partial class Ticket
    {
        public int id { get; set; }
        public string gebruikerid { get; set; }
        public Nullable<float> prijs { get; set; }
        public int Wedstrijdid { get; set; }
        public string Bezoekerrijksregisternummer { get; set; }
        public int Vakid { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Bezoeker Bezoeker { get; set; }
        public virtual Vak Vak { get; set; }
    }
}
