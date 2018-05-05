using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class VakService
    {
        private VakDAO vakDAO;


        public VakService()
        {
            vakDAO = new VakDAO();
        }


        public IEnumerable<Vak> GetVakkenInStadion(int stadionId)
        {
            return vakDAO.GetVakkenInStadion(stadionId);
        }
    }
}