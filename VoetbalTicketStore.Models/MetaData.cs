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

    public class WedstrijdMetadata
    {
        [Display(Name = "Wanneer")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime DatumEnTijd { get; set; }
        [Display(Name = "Thuisploeg")]
        public int Club1id { get; set; }
        [Display(Name = "Bezoekers")]
        public int Club2id { get; set; }
        [Display(Name = "Thuisploeg")]
        public virtual Club Club { get; set; }
        [Display(Name = "Bezoekers")]
        public virtual Club Club1 { get; set; }
    }
    public class BestellingMetadata
    {
        [Display(Name = "Wanneer")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BestelDatum { get; set; }
    }

    public class StadionMetadata
    {
        [Display(Name = "Stadion")]
        public String Naam { get; set; }
    }

    public class ClubMetadata
    {
        [Display(Name = "Club")]
        public String Naam { get; set; }
    }

    public class TicketMetadata
    {
        [Display(Name = "fuark")]

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Prijs { get; set; }
    }

    public class AbonnementMetadata
    {
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Prijs { get; set; }
    }
    public class ShoppingCartDataMetadata
    {
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Prijs { get; set; }
    }


}
