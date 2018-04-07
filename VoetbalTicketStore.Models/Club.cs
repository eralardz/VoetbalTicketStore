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
    
    public partial class Club
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Club()
        {
            this.Abonnements = new HashSet<Abonnement>();
            this.Wedstrijds = new HashSet<Wedstrijd>();
            this.Wedstrijds1 = new HashSet<Wedstrijd>();
        }
    
        public int id { get; set; }
        public string naam { get; set; }
        public int Stadionid { get; set; }
        public string logo { get; set; }
        public Nullable<float> ticketPrijsCoefficient { get; set; }
        public Nullable<float> abonnementPrijs { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Abonnement> Abonnements { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wedstrijd> Wedstrijds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wedstrijd> Wedstrijds1 { get; set; }
        public virtual Stadion Stadion { get; set; }
    }
}
