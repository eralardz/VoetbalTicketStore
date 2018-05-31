using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public interface IAbonnementService
    {
        int GetAantalAbonnementenPerVakVoorThuisPloeg(Club thuisploeg, Vak vak);
        IEnumerable<Abonnement> AddAbonnementen(IList<Abonnement> abonnementen);
        IEnumerable<Abonnement> GetNietGekoppeldeAbonnementen(string user);
        void KoppelBezoekerAanAbonnement(int teWijzigenAbonnement, string rijksregisternummer);
        Abonnement FindAbonnement(int teWijzigenAbonnement);
    }
}
