using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;
using System.Data.Entity;
using VoetbalTicketStore.DAO.Interfaces;

namespace VoetbalTicketStore.DAO
{
    public class AbonnementDAO : IAbonnementDAO
    {
        public int GetAantalAbonnementenPerVakVoorThuisPloeg(Club thuisploeg, Vak vak)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Abonnements.Count(a => a.Clubid == thuisploeg.Id && a.Vak.VakTypeid == vak.VakTypeid);
            }
        }

        public IEnumerable<Abonnement> AddAbonnementen(IList<Abonnement> abonnementen)
        {
            using (var db = new VoetbalstoreEntities())
            {
                db.Abonnements.AddRange(abonnementen);
                db.SaveChanges();

                return abonnementen;

            }
        }

        public IEnumerable<Abonnement> GetNietGekoppeldeAbonnementen(string user)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Abonnements.Where(a => a.AspNetUsersId.Equals(user) && a.Bezoekerrijksregisternummer == null).Include(c => c.Club).Include(s => s.Club.Stadion).Include(a => a.Vak.VakType).ToList();
            }
        }

        public void KoppelBezoekerAanAbonnement(Abonnement abonnement)
        {
            using (var db = new VoetbalstoreEntities())
            {
                db.Abonnements.Attach(abonnement);
                var entry = db.Entry(abonnement);
                entry.Property(e => e.Bezoekerrijksregisternummer).IsModified = true;
                db.SaveChanges();
            }
        }

        public Abonnement FindAbonnement(int teWijzigenAbonnement)
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Abonnements.Where(a => a.Id == teWijzigenAbonnement).Include(a => a.Club.Stadion).Include((a => a.Vak.VakType)).Include(a => a.Bestelling).Include(a => a.Bezoeker).FirstOrDefault();
            }
        }

        public void RemoveAbonnement(int teVerwijderenAbonnement)
        {
            if (teVerwijderenAbonnement > 0)
            {
                using (var db = new VoetbalstoreEntities())
                {
                    Abonnement toRemove = new Abonnement { Id = teVerwijderenAbonnement };
                    db.Entry(toRemove).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }
    }
}
