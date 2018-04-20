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
        public Vak FindVak(int selectedVakType, Stadion stadion)
        {
            using (var db = new VoetbalEntities())
            {
                Debug.WriteLine("FindVak");
                Debug.WriteLine("selected vak: " + selectedVakType);
                Debug.WriteLine("Stadion: " + stadion);

                return db.Vaks.Where(v => v.VakTypeid == selectedVakType && v.Stadionid == stadion.id).FirstOrDefault();
            }
        }


    }
}
