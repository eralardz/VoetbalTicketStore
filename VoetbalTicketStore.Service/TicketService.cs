using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Exceptions;
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

        public int GetAantalGekochteTickets(string user, int wedstrijdId)
        {
            return ticketDAO.GetAantalGekochteTickets(user, wedstrijdId);
        }

        public IEnumerable<IGrouping<Bestelling, Ticket>> GetNietGekoppeldeTickets(string user)
        {
            DateTime vanaf = DateTime.Now;
            return ticketDAO.GetNietGekoppeldeTickets(user, vanaf);
        }

        public void AddTickets(IList<Ticket> tickets)
        {
            ticketDAO.AddTickets(tickets);
        }

        public IEnumerable<Ticket> GetNietGekoppeldeTicketsList(string user)
        {
            return ticketDAO.GetNietGekoppeldeTicketsList(user).OrderBy(t => t.BestellingId);
        }

        public void KoppelBezoekerAanTicket(int teWijzigenTicket, string rijksregisternummer)
        {
            ticketDAO.KoppelBezoekerAanTicket(teWijzigenTicket, rijksregisternummer);
        }

        public Ticket FindTicket(int teWijzigenTicket)
        {
            return ticketDAO.FindTicket(teWijzigenTicket);
        }
    }
}
