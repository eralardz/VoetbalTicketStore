using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoetbalTicketStore.Service.Interfaces
{
    public interface IUserService
    {
        bool RijksregisternummerKomtAlVoor(string rijksregisternummer);
    }
}
