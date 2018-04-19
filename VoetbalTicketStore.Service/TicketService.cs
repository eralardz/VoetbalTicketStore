using System;
using System.Collections.Generic;
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
        private ZitplaatsDAO zitplaatsDAO;

        public TicketService()
        {
            ticketDAO = new TicketDAO();
        }

        public void BuyTicket(Ticket ticket, int selectedVakType)
        {
            // Is er plaats in dit vak? (maximaal aantal zitplaatsen - abonnementen - reeds verkochte tickets)
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
