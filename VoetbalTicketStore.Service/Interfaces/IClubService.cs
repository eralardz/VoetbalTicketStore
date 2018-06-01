using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service.Interfaces
{
    public interface IClubService
    {

        IEnumerable<Club> All();
        Club GetClub(int gekozenClubId);
       
    }
}
