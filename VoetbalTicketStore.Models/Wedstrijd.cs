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
    
    public partial class Wedstrijd
    {
        public int id { get; set; }
        public int Stadionid { get; set; }
        public int Club1id { get; set; }
        public int Club2id { get; set; }
        public System.DateTime DatumEnTijd { get; set; }
    
        public virtual Club Club { get; set; }
        public virtual Club Club1 { get; set; }
        public virtual Stadion Stadion { get; set; }
    }
}
