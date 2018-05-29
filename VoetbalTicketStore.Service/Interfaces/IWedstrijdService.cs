using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public interface IWedstrijdService
    {
        IEnumerable<Wedstrijd> All();
        IEnumerable<Wedstrijd> GetWedstrijdKalenderVanPloeg(Club club);
        IEnumerable<Wedstrijd> GetUpcomingWedstrijden();
        Wedstrijd GetWedstrijdById(int id);
        IEnumerable<Wedstrijd> GetAanTeRadenWedstrijdenVoorClub(int? clubId, int aantal);
    }
}
