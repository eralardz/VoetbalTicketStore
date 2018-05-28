using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public interface ITicketService
    {
        void AnnuleerTicket(int ticketId);
        int GetAantalVerkochteTicketsVoorVak(Vak vak, Wedstrijd wedstrijd);
        void AddTicket(Ticket ticket);
        int GetAantalGekochteTickets(string user, int wedstrijdId);
        IEnumerable<IGrouping<Bestelling, Ticket>> GetNietGekoppeldeTickets(string user);
        void AddTickets(IList<Ticket> tickets);
        IEnumerable<Ticket> GetNietGekoppeldeTicketsList(string user);
        void KoppelBezoekerAanTicket(int teWijzigenTicket, string rijksregisternummer);
        Ticket FindTicket(int teWijzigenTicket);
        int GetAantalTicketsVoorAndereWedstrijdOpDezelfdeDatum(string user, int wedstrijdId, DateTime datumEnTijd);
    }
}
