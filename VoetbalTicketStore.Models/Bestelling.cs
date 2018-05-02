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
    
    public partial class Bestelling
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bestelling()
        {
            this.ShoppingCartDatas = new HashSet<ShoppingCartData>();
            this.Tickets = new HashSet<Ticket>();
        }
    
        public int id { get; set; }
        public bool Bevestigd { get; set; }
        public decimal TotaalPrijs { get; set; }
        public string AspNetUsersId { get; set; }
        public System.DateTime BestelDatum { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShoppingCartData> ShoppingCartDatas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
