using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Exceptions;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Models.Constants;
using VoetbalTicketStore.Service.Interfaces;

namespace VoetbalTicketStore.Service
{
    public class VakService : IVakService
    {
        private VakDAO vakDAO;
        private TicketService ticketService;
        private AbonnementService abonnementService;


        public VakService()
        {
            vakDAO = new VakDAO();
        }


        public IEnumerable<Vak> GetVakkenInStadion(int stadionId)
        {
            if (stadionId < 0)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            return vakDAO.GetVakkenInStadion(stadionId);
        }

        public IEnumerable<Vak> GetThuisVakkeninStadion(int stadionId)
        {
            if (stadionId < 0)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            return vakDAO.GetThuisVakkenInStadion(stadionId);
        }

        // Bereken de specifieke prijs van het vak
        public void BerekenPrijzenBijVakken(IEnumerable<Vak> vakken, Club club)
        {
            if (vakken == null || club == null)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            foreach (Vak vak in vakken)
            {
                vak.BerekendePrijs = vak.VakType.StandaardPrijs * club.TicketPrijsCoefficient;
            }
        }

        public void BerekenAbonnementPrijzenBijVakken(IEnumerable<Vak> vakken, Club club)
        {
            if (vakken == null || club == null)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            foreach (Vak vak in vakken)
            {
                vak.BerekendePrijs = vak.VakType.StandaardPrijs * club.AbonnementPrijsCoefficient;
            }
        }

        public void BerekenAantalVrijePlaatsen(IEnumerable<Vak> vakken, Wedstrijd wedstrijd, Club thuisPloeg)
        {
            if (vakken == null || wedstrijd == null || thuisPloeg == null)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            ticketService = new TicketService();
            abonnementService = new AbonnementService();
            // tickets en abonnementen tellen
            foreach (Vak vak in vakken)
            {
                vak.AantalVrijePlaatsen = vak.MaximumAantalZitplaatsen - (ticketService.GetAantalVerkochteTicketsVoorVak(vak, wedstrijd) + abonnementService.GetAantalAbonnementenPerVakVoorThuisPloeg(thuisPloeg, vak));
            }
        }
    }
}