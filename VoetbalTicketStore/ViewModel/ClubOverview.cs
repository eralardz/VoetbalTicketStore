using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.ViewModel
{
    public class ClubOverview
    {
        [Display(Name = "Club")]
        public IList<Club> Clubs { get; set; }
        public int GekozenClubId { get; set; }
        public string GekozenClubNaam { get; set; }
        public int Stadionid { get; set; }
        [Display(Name = "Stadion")]
        public string StadionNaam { get; set; }
        [Display(Name = "Logo")]
        public string Logo { get; set; }
    }
}