using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO
{
    public class BestellingDAO
    {
        public Bestelling FindOpenstaandeBestelling(string user)
        {
            using (var db = new VoetbalstoreEntities())
            {
                // geeft null terug indien geen bestelling gevonden werd
                return db.Bestellings.Where(b => b.Bevestigd == false && b.AspNetUsersId.Equals(user)).FirstOrDefault();
            }
        }

        public Bestelling AddBestelling(Bestelling bestelling)
        {
            using (var db = new VoetbalstoreEntities())
            {
                db.Bestellings.Add(bestelling);
                db.SaveChanges();
                return bestelling;
            }
        }
    }
}
