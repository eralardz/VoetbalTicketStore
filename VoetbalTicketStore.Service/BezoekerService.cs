using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class BezoekerService
    {
        private BezoekerDAO bezoekerDAO;
        public Bezoeker FindBezoeker()
        {
            bezoekerDAO = new BezoekerDAO();
            return null;
        }
    }
}
