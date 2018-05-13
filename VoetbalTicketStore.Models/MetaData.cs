using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoetbalTicketStore.Models
{

    public class BezoekerMetadata
    {
        [StringLength(11)]
        [Display(Name = "Rijksregisternummer")]
        public string Rijksregisternummer;

        // TODO REGEX email
        [StringLength(50)]
        [Display(Name = "E-mailadres")]
        public string Email;

        [StringLength(50)]
        [Display(Name = "Middle Name")]
        public string Naam;

        [StringLength(50)]
        [Display(Name = "Voornaam")]
        public string Voornaam;
    }

}
