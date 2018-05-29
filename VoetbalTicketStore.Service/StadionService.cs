using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service.Interfaces;

namespace VoetbalTicketStore.Service
{
    public class StadionService : IStadionService
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
