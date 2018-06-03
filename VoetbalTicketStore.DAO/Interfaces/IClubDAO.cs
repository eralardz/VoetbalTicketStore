using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO.Interfaces
{
    public interface IClubDAO
    {
        IEnumerable<Club> All();
        Club GetClub(int gekozenClubId);
    }
}
