using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO
{
    public class BestellingDAO
    {

        public Bestelling FindOpenstaandeBestellingDoorUser(String user)
        {
            using (var db = new VoetbalEntities())
            {
               return db.Bestellings.Where(b => b.Bevestigd == false && b.AspNetUsersId.Equals(user)).FirstOrDefault();
            }

        }

        public void CreateNieuweBestelling(Bestelling bestelling)
        {
            using (var db = new VoetbalEntities())
            {
                try { 
                db.Bestellings.Add(bestelling);
                db.SaveChanges();
                } catch(Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}
