using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO.Interfaces
{
    public interface IBestellingDAO
    {
        Bestelling FindOpenstaandeBestelling(string user);
        Bestelling AddBestelling(Bestelling bestelling);
        Bestelling FindOpenstaandeBestellingDoorUser(string user);
        IEnumerable<Bestelling> All(string user);
        void RemoveBestelling(string user);
        void BevestigBestelling(Bestelling bestelling);
        int GetMeestGekochteThuisploeg(string user);
    }
}
