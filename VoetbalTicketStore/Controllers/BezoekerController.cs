using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.ViewModel;

namespace VoetbalTicketStore.Controllers
{
    public class BezoekerController : Controller
    {

        private BezoekerService bezoekerService;
        private BestellingService bestellingService;
        private TicketService ticketService;

        // GET: Bezoeker
        public ActionResult Index()
        {
            bezoekerService = new BezoekerService();
            bestellingService = new BestellingService();

            // Tickets en abonnementen van gebruiker zoeken die nog geen rijksregisternummer gekregen hebben, groeperen per bestelling

            // Tickets zonder koppeling
            ticketService = new TicketService();
            IEnumerable<IGrouping<Bestelling, Ticket>> tickets = ticketService.GetNietGekoppeldeTickets(User.Identity.GetUserId());

            // ViewModel maken en opvullen
            BezoekerKoppelen bezoekerKoppelen = new BezoekerKoppelen()
            {
                NietGekoppeldeTickets = tickets
            };

            return View(bezoekerKoppelen);
        }
    }
}