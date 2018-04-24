using System;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Controllers
{
    public class BestellingService
    {

        private BestellingDAO bestellingDAO;

        public Bestelling FindOpenstaandeBestellingDoorUser(string user)
        {
            bestellingDAO = new BestellingDAO();
            return bestellingDAO.FindOpenstaandeBestellingDoorUser(user);
        }

        public void CreateNieuweBestelling(decimal totaalPrijs, string user)
        {
            bestellingDAO = new BestellingDAO();

            Bestelling bestelling = new Bestelling();
            bestelling.TotaalPrijs = 0;
            bestelling.AspNetUsersId = user;

            bestellingDAO.CreateNieuweBestelling(bestelling);
        }
    }
}