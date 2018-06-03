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
    public class StadionDAO : IStadionDAO
    {
        public IEnumerable<Stadion> All()
        {
            using (var db = new VoetbalstoreEntities())
            {
                return db.Stadions.Include(s => s.Clubs).ToList();
            }
        }
    }
}
