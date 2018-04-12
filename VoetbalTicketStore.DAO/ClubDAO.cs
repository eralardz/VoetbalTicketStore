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
            var db = new VoetbalEntities();

            // lazy-loading
            // return db.Clubs; 

            // eager loading - voert een inner join uit tussen tabellen club en stadion
            return db.Clubs.Include(s => s.Stadion).ToList();
        }
    }
}
