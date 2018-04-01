using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO
{
    public class StadionDAO
    {
        public IEnumerable<Stadion> All()
        {
            var db = new VoetbalEntities();

            return db.Stadions; // lazy-loading
        }
    }
}
