using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public interface IBestellingService
    {
        Bestelling CreateNieuweBestellingIndienNodig(string user);
        IEnumerable<Bestelling> All(string user);
        Bestelling FindOpenstaandeBestellingDoorUser(string user);
        decimal BerekenTotaalPrijs(ICollection<ShoppingCartData> shoppingCartDatas);
        void RemoveBestelling(string user);
        void BevestigBestelling(int id, decimal totaalPrijs);
        int GetMeestGekochteThuisploeg(string user);
        void PlaatsBestelling(Bestelling bestelling, string user);
    }
}
