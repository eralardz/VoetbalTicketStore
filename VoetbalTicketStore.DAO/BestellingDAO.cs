using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;
using System.Data.Entity;

namespace VoetbalTicketStore.DAO
{
    public class BestellingDAO
    {

        public Bestelling FindOpenstaandeBestellingDoorUser(String user)
        {
            using (var db = new VoetbalEntities())
            {
                // lazy
                return db.Bestellings.Where(b => b.Bevestigd == false && b.AspNetUsersId.Equals(user)).FirstOrDefault();
            }

        }

        public int CreateNieuweBestelling(Bestelling bestelling)
        {
            using (var db = new VoetbalEntities())
            {
                db.Bestellings.Add(bestelling);
                db.SaveChanges();
                return bestelling.id;
            }
        }

        public Bestelling GetBestellingMetTicketsByUser(string user)
        {
            using (var db = new VoetbalEntities())
            {
                // eager
                // If you know you need related data for every entity retrieved, eager loading often offers the best performance, because a single query sent to the database is typically more efficient than separate queries for each entity retrieved.
                // https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/reading-related-data-with-the-entity-framework-in-an-asp-net-mvc-application

                return db.Bestellings.Where(b => b.Bevestigd == false && b.AspNetUsersId.Equals(user)).Include(t => t.Tickets).Include(s => s.ShoppingCartDatas).FirstOrDefault();
            }
        }
    }
}
