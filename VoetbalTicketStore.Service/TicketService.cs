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
    public class TicketService : ITicketService
    {
        private TicketDAO ticketDAO;

        public TicketService()
        {
            ticketDAO = new TicketDAO();
        }

        public int GetAantalVerkochteTicketsVoorVak(Vak vak, Wedstrijd wedstrijd)
        {
            if (vak == null || wedstrijd == null)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            return ticketDAO.GetAantalVerkochteTicketsVoorVak(vak, wedstrijd);

        }


        public void AddTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            ticketDAO.AddTicket(ticket);
        }

        public int GetAantalGekochteTickets(string user, int wedstrijdId)
        {
            if (user == null || wedstrijdId < 0)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            return ticketDAO.GetAantalGekochteTickets(user, wedstrijdId);
        }

        public IEnumerable<IGrouping<Bestelling, Ticket>> GetNietGekoppeldeTickets(string user)
        {
            if (user == null)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            DateTime vanaf = DateTime.Now;
            return ticketDAO.GetNietGekoppeldeTickets(user, vanaf);
        }

        public void AddTickets(IList<Ticket> tickets)
        {
            if (tickets == null)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            ticketDAO.AddTickets(tickets);
        }

        public void AnnuleerTicket(int ticketId)
        {
            if (ticketId < 0)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            // datum controleren (1 week op voorhand)
            Ticket ticket = FindTicket(ticketId);

            if ((ticket.Wedstrijd.DatumEnTijd - DateTime.Now).TotalDays > Constants.AantalDagenVoorWedstrijdAnnulerenToegestaan)
            {
                // ticket verwijderen          
                ticketDAO.RemoveTicket(ticketId);
            }
            else
            {
                throw new BestelException(Constants.TicketAnnuleren);
            }
        }

        public IEnumerable<Ticket> GetNietGekoppeldeTicketsList(string user)
        {
            if (user == null)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            return ticketDAO.GetNietGekoppeldeTicketsList(user).OrderBy(t => t.BestellingId);
        }

        public void KoppelBezoekerAanTicket(int teWijzigenTicket, string rijksregisternummer)
        {
            if (rijksregisternummer == null || teWijzigenTicket < 0)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            Ticket ticket = new Ticket { Id = teWijzigenTicket, Bezoekerrijksregisternummer = rijksregisternummer };

            ticketDAO.KoppelBezoekerAanTicket(ticket, rijksregisternummer);
        }

        public Ticket FindTicket(int teWijzigenTicket)
        {
            if (teWijzigenTicket < 0)
            {
                throw new BestelException(Constants.ParameterNull);

            }
            return ticketDAO.FindTicket(teWijzigenTicket);
        }

        public int GetAantalTicketsVoorAndereWedstrijdOpDezelfdeDatum(string user, int wedstrijdId, DateTime datumEnTijd)
        {
            if (user == null || datumEnTijd == null || wedstrijdId < 0)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            return ticketDAO.GetAantalTicketsVoorAndereWedstrijdOpDezelfdeDatum(user, wedstrijdId, datumEnTijd);
        }

        // Caution! Voorlopig enkel gebruikt voor testen.
        public void DeleteAlleTicketsVanUser(string user)
        {
            if (user == null)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            TicketDAO.DeleteAlleTicketsVanUser(user);
        }
    }
}
