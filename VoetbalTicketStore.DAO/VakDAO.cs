using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;
using System.Data.Entity;
using System.Diagnostics;

namespace VoetbalTicketStore.DAO
{
    public class VakDAO
    {
        public Vak FindVak(int selectedVakType, int stadionId)
        {
            Debug.WriteLine("vak");
            using (var db = new VoetbalEntities())
            {
                return db.Vaks.Where(v => v.VakTypeid == selectedVakType && v.Stadionid == stadionId).FirstOrDefault();
            }
        }
    }
}
