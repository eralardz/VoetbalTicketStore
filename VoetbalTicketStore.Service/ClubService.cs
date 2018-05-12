using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class ClubService
    {
        private ClubDAO clubDAO;

        public ClubService()
        {
            clubDAO = new ClubDAO();
        }

        // Geef alle clubs terug.
        public IEnumerable<Club> All()
        {
            return clubDAO.All();
        }

        public Club GetClub(int gekozenClubId)
        {
            return clubDAO.GetClub(gekozenClubId);
        }
    }
}
