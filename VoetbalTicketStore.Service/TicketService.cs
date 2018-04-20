using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class TicketService
    {
        private TicketDAO ticketDAO;
        private VakDAO vakDAO;

        public TicketService()
        {
            ticketDAO = new TicketDAO();
            vakDAO = new VakDAO();
        }

        public void BuyTicket(Ticket ticket, int selectedVakType, Stadion stadion, Wedstrijd wedstrijd)
        {
            // Is er plaats in dit vak? (maximaal aantal zitplaatsen - abonnementen - reeds verkochte tickets)

            // Hoe ziet dit vaktype eruit in dit stadion?
            // Zoeken naar vak in dit stadion

            // We kennen het stadion en het vaktype... VIND HET SPECIFIEKE VAK MET ZIJN ID

            Debug.WriteLine("BuyTicket");

            Vak vak = vakDAO.FindVak(selectedVakType, stadion);

            Debug.WriteLine(vak.maximumAantalZitplaatsen);


            ticketDAO.AddTicket(ticket);



        }

        private bool IsVakVrij()
        {
            // iets doen met de ZitPlaatsService

            // Count van zitplaatsen group by vakID



            return false;
        }


    }
}
