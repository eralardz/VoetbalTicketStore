using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;
using System.Data.Entity;


namespace VoetbalTicketStore.DAO
{
    public class WedstrijdDAO
    {
        public IEnumerable<Wedstrijd> All()
        {
            //In general, if you're going to use the result of a query more than once, it's always a good idea to store it via ToList() or ToArray(). 
            //This is especially true if you're LINQ query is an "expensive" one, as it prevents the expensive operation from running more than once.
            var db = new VoetbalEntities();
            return db.Wedstrijds.ToList(); // lazy-loading
        }

        public IEnumerable<Wedstrijd> getWedstrijdKalenderVanPloeg(Club club)
        {
            using (var db = new VoetbalEntities())
            {
                // standard .NET boolean operators are supported in a where clause
                return db.Wedstrijds.Where(w => w.Club1id == club.id || w.Club2id == club.id).Include(c => c.Club).Include(c => c.Club1).Include(s => s.Stadion).ToList();
            }
        }

        // Find is a DbSet method that first tries to find the requested entity in the context's cache. Only when it's not found there, the entity is fetched from the database.
        // Because of this special behavior(of Find), Include and Find can't be mixed.
        public Wedstrijd getWedstrijdById(int id)
        {
            using (var db = new VoetbalEntities())
            {
                // FirstOrDefault -> If it can return null (as in ID not found or something)
                return db.Wedstrijds.Where(w => w.id == id).Include(s => s.Stadion).Include(c => c.Club).Include(c => c.Club1).FirstOrDefault();
            }
        }
    }
}
