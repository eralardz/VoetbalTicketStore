using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO
{
    public class ClubDAO
    {
        public IEnumerable<Club> All()
        {
            // The using statement calls the Dispose method on the object in the correct way, and (when you use it as shown earlier) it also causes the object itself to go out of scope as soon as Dispose is called. Within the using block, the object is read-only and cannot be modified or reassigned. 
            using (var db = new VoetbalstoreEntities())
            {

                // lazy-loading
                // return db.Clubs; 

                // eager loading - voert een inner join uit tussen tabellen club en stadion
                return db.Clubs.Include(s => s.Stadion).ToList();
            }
        }

        public Club GetClub(int gekozenClubId)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Clubs.Find(gekozenClubId);
            }
        }
    }
}
