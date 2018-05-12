using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO
{
    public class AbonnementDAO
    {
        public int GetAantalAbonnementenPerVakVoorThuisPloeg(Club thuisploeg, Vak vak)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Abonnements.Count(a => a.Clubid == thuisploeg.Id && a.VakTypeId == vak.VakTypeid);
            }
        }

        public void AddAbonnementen(IList<Abonnement> abonnementen)
        {
            using (var db = new VoetbalstoreEntities())
            {
                db.Abonnements.AddRange(abonnementen);
                db.SaveChanges();
            }
        }
    }
}
