using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class ShoppingCartDataService
    {
        private TicketService ticketService;

        private ShoppingCartDataDAO shoppingCartDataDAO;
        private TicketDAO ticketDAO;

        public void AddShoppingCartData(string user, Ticket ticket, int bestellingId, int wedstrijdId)
        {
            if (MagGebruikerNogEenTicketToevoegen(user, wedstrijdId)) { 
            ShoppingCartData shoppingCartData = new ShoppingCartData();
            shoppingCartData.Ticketid = ticket.id;
            shoppingCartData.BestellingId = bestellingId;
            shoppingCartData.prijs = ticket.prijs;

            shoppingCartDataDAO = new ShoppingCartDataDAO();
            shoppingCartDataDAO.AddShoppingCartData(shoppingCartData);
            }
            else
            {
                throw new TeveelTicketsException("Er mogen slechts 4 tickets per wedstrijd besteld worden!");
            }
        }

        public decimal berekenTotaalPrijs(Bestelling bestelling)
        {
            decimal totaalprijs = 0;
            foreach(ShoppingCartData s in bestelling.ShoppingCartDatas)
            {
                totaalprijs += s.prijs;
            }

            return totaalprijs;
        }

        public void RemoveShoppingCartData(int shoppingCartDataId)
        {
            // vind ticket gelinkt aan shoppingcart
            ticketService = new TicketService();

            // TODO: database hit vermijden door gebruik te maken van hidden field
            int ticketId = ticketService.FindTicketBasedOnShoppingCartId(shoppingCartDataId);

            // verwijderen shoppingcartdata
            shoppingCartDataDAO = new ShoppingCartDataDAO();
            shoppingCartDataDAO.RemoveShoppingCartData(shoppingCartDataId);

            // verwijderen ticket
            ticketService.RemoveTicket(ticketId);

        }

        private bool MagGebruikerNogEenTicketToevoegen(string user, int wedstrijdId)
        {
            // Een gebruiker mag maximaal 4 tickets bestellen per wedstrijd. Dit wordt gecontroleerd in de TicketDAO.
            ticketDAO = new TicketDAO();
            return (ticketDAO.GetHoeveelheidTickets(user, wedstrijdId) <= TicketService.MaximumAantalTicketsPerGebruikerPerWedstrijd);
        }
    }
}
