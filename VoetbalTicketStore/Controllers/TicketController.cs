using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.Controllers
{
    public class TicketController : Controller
    {

        private TicketService ticketService;
        private WedstrijdService wedstrijdService;
             
        // GET: Ticket
        public ActionResult Buy(int id)
        {
            wedstrijdService = new WedstrijdService();
            Wedstrijd wedstrijd = wedstrijdService.getWedstrijdById(id);

            Debug.WriteLine("Stadion: " + wedstrijd.Stadionid);
            Debug.WriteLine("Club 1: " + wedstrijd.Club1id);
            Debug.WriteLine("Club 2: " + wedstrijd.Club2id);
            return View();
        }
    }
}