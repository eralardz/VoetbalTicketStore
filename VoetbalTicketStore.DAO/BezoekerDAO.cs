using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO
{
    public class BezoekerDAO
    {
        public Bezoeker FindBezoeker(string rijksregisternummer)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Bezoekers.Find(rijksregisternummer);
            }
        }

        public void AddBezoeker(Bezoeker bezoeker)
        {
            using (var db = new VoetbalstoreEntities())
            {
                db.Bezoekers.Add(bezoeker);
                db.SaveChanges();
            }
        }
    }
}
