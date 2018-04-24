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

        public TicketService()
        {
            ticketDAO = new TicketDAO();
            vakDAO = new VakDAO();
            VakTypeDAO = new VakTypeDAO();
        }

        public void BuyTicket(int selectedVakType, int stadionId, int wedstrijdId, string user, string rijksregisternummer)
        {
            // Is er plaats in dit vak? (maximaal aantal zitplaatsen - abonnementen - reeds verkochte tickets)

            // Hoe ziet dit vaktype eruit in dit stadion?
            // Zoeken naar vak in dit stadion

            // We kennen het stadion en het vaktype... VIND HET SPECIFIEKE VAK MET ZIJN ID
            Vak vak = vakDAO.FindVak(selectedVakType, stadionId);

            if(IsVakVrij(vak.id, wedstrijdId, vak.maximumAantalZitplaatsen))
            {
                Ticket ticket = new Ticket();
                ticket.gebruikerid = user;
                ticket.Bezoekerrijksregisternummer = rijksregisternummer;
                ticket.Wedstrijdid = wedstrijdId;
                ticket.Vakid = vak.id;
                ticket.prijs = BepaalPrijs(vak);


            }
        }

        private float BepaalPrijs(Vak vak)
        {
            // prijs wordt bepaald door vak en club
            VakType vakType = VakTypeDAO.FindVakType(vak.id);
            Debug.WriteLine("Standaardprijs voor dit vak: " + vakType.standaardPrijs);

            return 0;
        }

        // Is een vak vrij? 
        private bool IsVakVrij(int vakId, int wedstrijdId, int maximumAantalZitplaatsen)
        {
            int aantalVerkochteTickets = ticketDAO.FindVerkochteTicketsVakPerWedstrijd(vakId, wedstrijdId);
            return aantalVerkochteTickets < maximumAantalZitplaatsen;
        }


    }
}
