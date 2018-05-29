using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service.Interfaces
{
    public interface IVakService
    {
        IEnumerable<Vak> GetVakkenInStadion(int stadionId);
        IEnumerable<Vak> GetThuisVakkeninStadion(int stadionId);
        void BerekenPrijzenBijVakken(IEnumerable<Vak> vakken, Club club);
        void BerekenAbonnementPrijzenBijVakken(IEnumerable<Vak> vakken, Club club);
        void BerekenAantalVrijePlaatsen(IEnumerable<Vak> vakken, Wedstrijd wedstrijd, Club thuisploeg);
    }
}
