using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO.Interfaces
{
    public interface IVakTypeDAO
    {
        IEnumerable<VakType> All();
        VakType FindVakType(int id);
    }
}
