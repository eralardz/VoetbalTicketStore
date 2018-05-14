using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models.DataAnnotations;

namespace VoetbalTicketStore.Models
{

    public class BezoekerMetadata
    {
        [Required]
        [StringLength(11)]
        [Rijksregisternummer]
        // TODO: hier culture ingeven? Met constants ofzo.
        [Display(Name = "Rijksregisternummer")]
        public string Rijksregisternummer;

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Dit is geen correct gevormd e-mailadres!")]
        [Required]
        [Display(Name = "E-mailadres")]
        public string Email;

        [Required]
        [Display(Name = "Familienaam")]
        public string Naam;

        [Required]
        [Display(Name = "Voornaam")]
        public string Voornaam;
    }

}
