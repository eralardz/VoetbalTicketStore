﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.DAO
{
    public class TicketDAO
    {
        public IEnumerable<Ticket> All()
        {
            var db = new VoetbalEntities();

            return db.Tickets; // lazy-loading
        }

        public void AddTicket(Ticket ticket)
        {
            using (var db = new VoetbalEntities())
            {
                db.Tickets.Add(ticket);
                try { 
                db.SaveChanges();
                }
                
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.InnerException);
                }
                }

        }

        public int FindVerkochteTicketsVakPerWedstrijd(int vakId, int wedstrijdId)
        {
            using (var db = new VoetbalEntities())
            {
                return db.Tickets.Count(t => t.Vakid == vakId && t.Wedstrijdid == wedstrijdId);
            }
        }
    }
}
