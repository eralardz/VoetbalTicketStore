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

        public int GetAantalVerkochteTicketsVoorVak(Vak vak, Wedstrijd wedstrijd)
        {
            return ticketDAO.GetAantalVerkochteTicketsVoorVak(vak, wedstrijd);
        }

        public void AddTicket(Ticket ticket)
        {
            ticketDAO.AddTicket(ticket);
        }
    }
}
