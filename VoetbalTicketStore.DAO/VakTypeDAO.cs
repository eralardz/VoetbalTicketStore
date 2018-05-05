using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO
{
    public class VakTypeDAO
    {
        public IEnumerable<VakType> All()
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.VakTypes.ToList(); // lazy-loading
            }
        }

        public VakType FindVakType(int id)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.VakTypes.Find(id); // lazy-loading
            }
        }
    }
}
