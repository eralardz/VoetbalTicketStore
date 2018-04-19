using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class VakTypeService 
    {
        private VakTypeDAO vakTypeDAO;


        public IEnumerable<VakType> All()
        {
            vakTypeDAO = new VakTypeDAO();
            return vakTypeDAO.All();
        }
    }
}
