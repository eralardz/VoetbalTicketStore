using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO.Interfaces
{
    public interface IAbonnementDAO
    {
        int GetAantalAbonnementenPerVakVoorThuisPloeg(Club thuisploeg, Vak vak);
        IEnumerable<Abonnement> AddAbonnementen(IList<Abonnement> abonnementen);
        IEnumerable<Abonnement> GetNietGekoppeldeAbonnementen(string user);
        void KoppelBezoekerAanAbonnement(Abonnement abonnement);
        Abonnement FindAbonnement(int teWijzigenAbonnement);
        void RemoveAbonnement(int teVerwijderenAbonnement);

    }
}
