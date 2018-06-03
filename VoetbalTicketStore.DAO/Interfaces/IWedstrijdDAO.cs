using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO.Interfaces
{
    public interface IWedstrijdDAO
    {
        IEnumerable<Wedstrijd> All();
        IEnumerable<Wedstrijd> GetUpcomingWedstrijden();
        IEnumerable<Wedstrijd> GetAanTeRadenWedstrijdenVoorClub(int? clubId, int aantal);
        IEnumerable<Wedstrijd> getWedstrijdKalenderVanPloeg(Club club);
        Wedstrijd getWedstrijdById(int id);
    }
}
