using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoetbalTicketStore.Models
{
    public partial class Vak
    {
        [NotMapped]
        public decimal BerekendePrijs { get; set; }
        [NotMapped]
        public int AantalVrijePlaatsen { get; set; }
    }
}
