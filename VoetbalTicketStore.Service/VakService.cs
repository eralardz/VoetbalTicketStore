using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class VakService
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
            return vakDAO.GetVakkenInStadion(stadionId);
        }

        // Bereken de specifieke prijs van het vak
        public void BerekenPrijzenBijVakken(IEnumerable<Vak> vakken, Club club)
        {
            foreach (Vak vak in vakken)
            {
                vak.BerekendePrijs = vak.VakType.StandaardPrijs * club.TicketPrijsCoefficient;
            }
        }

        public void BerekenAantalVrijePlaatsen(IEnumerable<Vak> vakken, Wedstrijd wedstrijd, Club thuisploeg)
        {
            ticketService = new TicketService();
            abonnementService = new AbonnementService();
            // tickets en abonnementen tellen
            foreach (Vak vak in vakken)
            {
                vak.AantalVrijePlaatsen = vak.MaximumAantalZitplaatsen - (ticketService.GetAantalVerkochteTicketsVoorVak(vak, wedstrijd) + abonnementService.GetAantalAbonnementenPerVakVoorThuisPloeg(thuisploeg, vak));
            }
        }
    }
}