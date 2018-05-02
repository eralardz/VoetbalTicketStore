using System;
using System.Diagnostics;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class TicketService
    {
        // DAO's
        private TicketDAO ticketDAO;
        private VakDAO vakDAO;
        private VakTypeDAO VakTypeDAO;
        private WedstrijdDAO wedstrijdDAO;
        private ShoppingCartDataDAO shoppingCartDataDAO;

        // constanten
        public const int MaximumAantalTicketsPerGebruikerPerWedstrijd = 4;

        public TicketService()
        {
            ticketDAO = new TicketDAO();
            vakDAO = new VakDAO();
            VakTypeDAO = new VakTypeDAO();
            wedstrijdDAO = new WedstrijdDAO();
            shoppingCartDataDAO = new ShoppingCartDataDAO();
        }

        public Ticket BuyTicket(int bestellingId, int selectedVakType, int stadionId, int wedstrijdId, string user, string rijksregisternummer)
        {

            // Mag gebruiker nog een ticket toevoegen?
            if (MagGebruikerNogEenTicketToevoegen(user, wedstrijdId))
            {
                // Is er plaats in dit vak? (maximaal aantal zitplaatsen - abonnementen - reeds verkochte tickets)

                Vak vak = vakDAO.FindVak(selectedVakType, stadionId);

                Ticket ticket = new Ticket();

                if (IsVakVrij(vak.id, wedstrijdId, vak.maximumAantalZitplaatsen))
                {
                    ticket.gebruikerid = user;
                    ticket.Bezoekerrijksregisternummer = rijksregisternummer;
                    ticket.Wedstrijdid = wedstrijdId;
                    ticket.Vakid = vak.id;
                    ticket.prijs = BepaalPrijs(vak, wedstrijdId);
                    ticket.BestellingId = bestellingId;
                }
                return ticketDAO.AddTicket(ticket);
            }
            else
            {
                throw new TeveelTicketsException("Er mogen slechts 4 tickets per wedstrijd besteld worden!");
            }
        }

        public void RemoveTicket(int ticketId)
        {
            ticketDAO.RemoveTicket(ticketId);
        }

        public int FindTicketBasedOnShoppingCartId(int shoppingCartDataId)
        {
            ShoppingCartData shoppingCartData = shoppingCartDataDAO.FindShoppingCartData(shoppingCartDataId);
            return shoppingCartData.Ticketid;
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

        private bool MagGebruikerNogEenTicketToevoegen(string user, int wedstrijdId)
        {
            // Een gebruiker mag maximaal 4 tickets bestellen per wedstrijd. Dit wordt gecontroleerd in de TicketDAO.
            ticketDAO = new TicketDAO();
            return (ticketDAO.GetHoeveelheidTickets(user, wedstrijdId) < MaximumAantalTicketsPerGebruikerPerWedstrijd);
        }
    }
}
