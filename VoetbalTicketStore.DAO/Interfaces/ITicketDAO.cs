using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO.Interfaces
{
    public interface ITicketDAO
    {
        int GetAantalVerkochteTicketsVoorVak(Vak vak, Wedstrijd wedstrijd);
        void AddTicket(Ticket ticket);
        int GetAantalGekochteTickets(string user, int wedstrijdId);
        void AddTickets(IList<Ticket> tickets);
        IEnumerable<IGrouping<Bestelling, Ticket>> GetNietGekoppeldeTickets(string user, DateTime vanaf);
        void RemoveTicket(int ticketId);
        Ticket FindTicket(int teWijzigenTicket);
        int GetAantalTicketsVoorAndereWedstrijdOpDezelfdeDatum(string user, int wedstrijdId, DateTime datumEnTijd);
        void KoppelBezoekerAanTicket(Ticket ticket, string rijksregisternummer);
        IList<Ticket> GetNietGekoppeldeTicketsList(string user);
    }
}
