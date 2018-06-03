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
    public class VakDAO : IVakDAO
    {

        public IEnumerable<Vak> GetVakkenInStadion(int stadionId)
        {
            using (var db = new VoetbalstoreEntities())
            {
                // eager
                return db.Vaks.Where(v => v.Stadionid == stadionId).Include(v => v.VakType).ToList();
            } 
        }

        public IEnumerable<Vak> GetThuisVakkenInStadion(int stadionId)
        {
            using (var db = new VoetbalstoreEntities())
            {
                // eager
                return db.Vaks.Where(v => v.Stadionid == stadionId && v.VakType.ThuisVak == true).Include(v => v.VakType).ToList();
            }
        }
    }
}
