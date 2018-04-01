using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class StadionService
    {
        private StadionDAO stadionDAO;

        public StadionService()
        {
            stadionDAO = new StadionDAO();
        }

        public IEnumerable<Stadion> All()
        {
            return stadionDAO.All();
        }
    }
}
