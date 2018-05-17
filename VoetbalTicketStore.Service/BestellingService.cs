using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class BestellingService
    {
        private BestellingDAO bestellingDAO;

        public BestellingService()
        {
            bestellingDAO = new BestellingDAO();
        }

        public Bestelling CreateNieuweBestellingIndienNodig(string user)
        {
            Bestelling gevondenBestelling = bestellingDAO.FindOpenstaandeBestelling(user);

            if (gevondenBestelling != null)
            {
                // openstaande bestelling teruggeven
                return gevondenBestelling;

            }
            else
            {
                // nieuwe bestelling aanmaken
                Bestelling bestelling = new Bestelling()
                {
                    Bevestigd = false,
                    TotaalPrijs = 0,
                    AspNetUsersId = user,
                    BestelDatum = DateTime.Now
                };

                return bestellingDAO.AddBestelling(bestelling);
            }
        }

        public Bestelling FindOpenstaandeBestellingDoorUser(string user)
        {
            return bestellingDAO.FindOpenstaandeBestellingDoorUser(user);
        }

        public decimal BerekenTotaalPrijs(ICollection<ShoppingCartData> shoppingCartDatas)
        {
            decimal totaalPrijs = 0;

            foreach (ShoppingCartData shoppingCartData in shoppingCartDatas)
            {
                totaalPrijs += (shoppingCartData.Prijs * shoppingCartData.Hoeveelheid);
            }

            return totaalPrijs;
        }

        public void RemoveBestelling(string user)
        {
            bestellingDAO.RemoveBestelling(user);
        }

        public void BevestigBestelling(int id)
        {
            Bestelling bestelling = new Bestelling { Id = id, Bevestigd = true };
            bestellingDAO.BevestigBestelling(bestelling);
        }
    }
}
