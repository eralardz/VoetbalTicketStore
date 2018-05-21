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
        private AbonnementService abonnementService;

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

        public void AnnuleerTicket(int ticketId)
        {
            // datum controleren (1 week op voorhand)
            Ticket ticket = FindTicket(ticketId);

            if((ticket.Wedstrijd.DatumEnTijd - DateTime.Now).TotalDays > 7)
            {
                // ticket verwijderen          
                ticketDAO.RemoveTicket(ticketId);    

            } else
            {
                throw new BestelException("Een ticket mag ten laatste 7 dagen op voorhand geannuleerd worden!");
            }
        }

        public IEnumerable<Ticket> GetNietGekoppeldeTicketsList(string user)
        {
            return ticketDAO.GetNietGekoppeldeTicketsList(user).OrderBy(t => t.BestellingId);
        }

        // TODO: Ticket-object hier maken ipv in DAO (moet eigenlijk hier...)
        public void KoppelBezoekerAanTicket(int teWijzigenTicket, string rijksregisternummer)
        {
            ticketDAO.KoppelBezoekerAanTicket(teWijzigenTicket, rijksregisternummer);
        }

        public Ticket FindTicket(int teWijzigenTicket)
        {
            return ticketDAO.FindTicket(teWijzigenTicket);
        }

        public int GetAantalTicketsVoorAndereWedstrijdOpDezelfdeDatum(string user, int wedstrijdId, DateTime datumEnTijd)
        {
            return ticketDAO.GetAantalTicketsVoorAndereWedstrijdOpDezelfdeDatum(user, wedstrijdId, datumEnTijd);
        }
    }
}
