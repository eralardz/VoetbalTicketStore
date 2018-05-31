using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Exceptions;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Models.Constants;

namespace VoetbalTicketStore.Service
{
    public class AbonnementService : IAbonnementService
    {
        private AbonnementDAO abonnementDAO;

        public AbonnementService()
        {
            abonnementDAO = new AbonnementDAO();
        }

        public int GetAantalAbonnementenPerVakVoorThuisPloeg(Club thuisploeg, Vak vak)
        {
            if (thuisploeg != null && vak != null)
            {
                return abonnementDAO.GetAantalAbonnementenPerVakVoorThuisPloeg(thuisploeg, vak);
            }
            else
            {
                throw new BestelException(Constants.ParameterNull);
            }
        }

        public IEnumerable<Abonnement> AddAbonnementen(IList<Abonnement> abonnementen)
        {
            if (abonnementen != null)
            {
                return abonnementDAO.AddAbonnementen(abonnementen);
            }

            else
            {
                throw new BestelException(Constants.ParameterNull);
            }
        }

        public IEnumerable<Abonnement> GetNietGekoppeldeAbonnementen(string user)
        {
            if (user != null)
            {
                return abonnementDAO.GetNietGekoppeldeAbonnementen(user);
            }
            else
            {
                throw new BestelException(Constants.ParameterNull);
            }
        }

        public void KoppelBezoekerAanAbonnement(int teWijzigenAbonnement, string rijksregisternummer)
        {
            if (teWijzigenAbonnement >= 0)
            {
                Abonnement abonnement = new Abonnement()
                {
                    Id = teWijzigenAbonnement,
                    Bezoekerrijksregisternummer = rijksregisternummer
                };

                abonnementDAO.KoppelBezoekerAanAbonnement(abonnement);
            }
            else
            {
                throw new BestelException(Constants.ParameterNull);
            }
        }

        public Abonnement FindAbonnement(int teWijzigenAbonnement)
        {
            if (teWijzigenAbonnement >= 0)
            {
                return abonnementDAO.FindAbonnement(teWijzigenAbonnement);
            }
            else
            {
                throw new BestelException(Constants.ParameterNull);
            }
        }

        public void RemoveAbonnement(int teVerwijderenAbonnement)
        {
            if (teVerwijderenAbonnement >= 0)
            {
                abonnementDAO.RemoveAbonnement(teVerwijderenAbonnement);
            }
        }
    }
}