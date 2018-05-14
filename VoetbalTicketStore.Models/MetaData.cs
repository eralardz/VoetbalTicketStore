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
        [Required]
        [StringLength(11)]
        // TODO: hier culture ingeven? Met constants ofzo.
        [Display(Name = "Rijksregisternummer")]
        public string Rijksregisternummer;

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Dit is geen correct gevormd e-mailadres!")]
        [Required]
        [StringLength(50)]
        [Display(Name = "E-mailadres")]
        public string Email;

        [Required]
        [StringLength(50)]
        [Display(Name = "Familienaam")]
        public string Naam;

        [Required]
        [StringLength(50)]
        [Display(Name = "Voornaam")]
        public string Voornaam;
    }

}
