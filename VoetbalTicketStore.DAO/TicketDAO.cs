using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO
{
    public class TicketDAO
    {

        public int GetAantalVerkochteTicketsVoorVak(Vak vak, Wedstrijd wedstrijd)
        {
            using(var db = new VoetbalstoreEntities())
            {
                return db.Tickets.Count(t => t.Vakid == vak.Id && t.Wedstrijdid == wedstrijd.Id);
            }
        }
    }
}
