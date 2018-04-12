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

        public TicketService()
        {
            ticketDAO = new TicketDAO();
        }

        public void addTicket(Ticket ticket)
        {
            ticketDAO.AddTicket(ticket);
        }

        private int getFreeSeat()
        {
            // iets doen met de ZitPlaatsService
            return 0;
        }
    }
}
