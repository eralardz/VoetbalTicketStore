using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.ViewModel
{
    public class ClubOverview
    {
        public IList<Club> Clubs { get; set; }

        public int GekozenClubId { get; set; }
        public string GekozenClubNaam { get; set; }

        public int Stadionid { get; set; }
        public string StadionNaam { get; set; }
        public string Logo { get; set; }
    }
}