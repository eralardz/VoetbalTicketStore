using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO
{
    public class ZitplaatsDAO
    {
        public IEnumerable<Zitplaat> All()
        {
            using (var db = new VoetbalEntities())
            {
                return db.Zitplaats.ToList(); // lazy-loading
            }
        }


        // TODO: voortwerken - COUNT alle zitplaatsen met een bepaald vakid
        public Zitplaat GetAantalZitplaatsenInVak()
        {
            return 0;
        }
    }
}
