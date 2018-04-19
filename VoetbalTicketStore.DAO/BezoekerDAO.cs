using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO
{
    public class BezoekerDAO
    {
        public Bezoeker FindBezoeker()
        {
            return null;
        }

        public void AddBezoeker(Bezoeker bezoeker)
        {
            using (var db = new VoetbalEntities())
            {

                Debug.WriteLine(bezoeker.rijksregisternummer);
                Debug.WriteLine(bezoeker.naam);
                Debug.WriteLine(bezoeker.voornaam);
                Debug.WriteLine(bezoeker.email);




                db.Bezoekers.Add(bezoeker);
                db.SaveChanges();
            }
        }

        public Bezoeker FindBezoeker(string rijksregisternummer)
        {
            using (var db = new VoetbalEntities())
            {
                return db.Bezoekers.Find(rijksregisternummer);
            }
        }

        public Bezoeker BestaatBezoeker(Bezoeker bezoeker)
        {

            using (var db = new VoetbalEntities())
            {
            }
            return null;
        }

    }
}
