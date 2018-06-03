using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO.Interfaces
{
    public interface IVakDAO
    {
        IEnumerable<Vak> GetVakkenInStadion(int stadionId);
        IEnumerable<Vak> GetThuisVakkenInStadion(int stadionId);

    }
}
