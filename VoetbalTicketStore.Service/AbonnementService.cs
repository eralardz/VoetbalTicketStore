using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class AbonnementService
    {
        private AbonnementDAO abonnementDAO;

        public AbonnementService()
        {
            abonnementDAO = new AbonnementDAO();
        }

        public int GetAantalAbonnementenPerVakVoorThuisPloeg(Club thuisploeg, Vak vak)
        {
            return abonnementDAO.GetAantalAbonnementenPerVakVoorThuisPloeg(thuisploeg, vak);
        }

        public void AddAbonnementen(IList<Abonnement> abonnementen)
        {
            abonnementDAO.AddAbonnementen(abonnementen);
        }
    }
}
