using System.Diagnostics;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class TicketService
    {
        private TicketDAO ticketDAO;
        private VakDAO vakDAO;
        private VakTypeDAO VakTypeDAO;
        private WedstrijdDAO wedstrijdDAO;

        public TicketService()
        {
            ticketDAO = new TicketDAO();
            vakDAO = new VakDAO();
            VakTypeDAO = new VakTypeDAO();
            wedstrijdDAO = new WedstrijdDAO();
        }

        public void BuyTicket(int selectedVakType, int stadionId, int wedstrijdId, string user, string rijksregisternummer)
        {
            // Is er plaats in dit vak? (maximaal aantal zitplaatsen - abonnementen - reeds verkochte tickets)

            // Hoe ziet dit vaktype eruit in dit stadion?
            // Zoeken naar vak in dit stadion

            // We kennen het stadion en het vaktype... VIND HET SPECIFIEKE VAK MET ZIJN ID
            Vak vak = vakDAO.FindVak(selectedVakType, stadionId);

            if (IsVakVrij(vak.id, wedstrijdId, vak.maximumAantalZitplaatsen))
            {
                Ticket ticket = new Ticket();
                ticket.gebruikerid = user;
                ticket.Bezoekerrijksregisternummer = rijksregisternummer;
                ticket.Wedstrijdid = wedstrijdId;
                ticket.Vakid = vak.id;
                ticket.prijs = BepaalPrijs(vak, wedstrijdId);
                ticketDAO.AddTicket(ticket);
            }

        }

        private decimal BepaalPrijs(Vak vak, int wedstrijdId)
        {
            // prijs wordt bepaald door vaktype en club
            VakType vakType = VakTypeDAO.FindVakType(vak.VakTypeid);

            decimal standaardPrijs = vakType.standaardPrijs;

            // null-coalescing operator -> It returns the left-hand operand if the operand is not null; otherwise it returns the right hand operand.
            // Want bool is nullable... TODO eventueel Nullable afzetten, dan komen we dit probleem niet tegen.
            Wedstrijd wedstrijd = wedstrijdDAO.getWedstrijdById(wedstrijdId);

            // 'Club' is altijd de thuisploeg
            decimal coefficient = wedstrijd.Club.ticketPrijsCoefficient;

            return standaardPrijs * coefficient;
        }

        // Is een vak vrij? 
        private bool IsVakVrij(int vakId, int wedstrijdId, int maximumAantalZitplaatsen)
        {
            int aantalVerkochteTickets = ticketDAO.FindVerkochteTicketsVakPerWedstrijd(vakId, wedstrijdId);
            return aantalVerkochteTickets < maximumAantalZitplaatsen;
        }


    }
}
