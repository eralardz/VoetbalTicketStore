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


        public VakTypeService()
        {
            vakTypeDAO = new VakTypeDAO();
        }


        public IEnumerable<VakType> All()
        {
            return vakTypeDAO.All();
        }
    }
}