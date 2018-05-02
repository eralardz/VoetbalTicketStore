using System;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.Controllers
{
    public class BestellingService
    {

        private BestellingDAO bestellingDAO;
        private TicketService ticketService;


        public Bestelling FindOpenstaandeBestellingDoorUser(string user)
        {
            bestellingDAO = new BestellingDAO();
            return bestellingDAO.FindOpenstaandeBestellingDoorUser(user);
        }

        public int CreateNieuweBestelling(decimal totaalPrijs, string user)
        {
            bestellingDAO = new BestellingDAO();

            Bestelling bestelling = new Bestelling();
            bestelling.TotaalPrijs = 0;
            bestelling.AspNetUsersId = user;
            bestelling.BestelDatum = DateTime.Now;

            return bestellingDAO.CreateNieuweBestelling(bestelling);
        }

        public Bestelling GetBestellingMetTicketsByUser(string user)
        {
            bestellingDAO = new BestellingDAO();
            return bestellingDAO.GetBestellingMetTicketsByUser(user);
        }
    }
}