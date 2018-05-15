using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.ViewModel;

namespace VoetbalTicketStore.Controllers
{
    [Authorize]
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

            //// WERKWIJZE MET IGROUPING
            //// Tickets zonder koppeling
            //ticketService = new TicketService();
            //IEnumerable<IGrouping<Bestelling, Ticket>> tickets = ticketService.GetNietGekoppeldeTickets(User.Identity.GetUserId());

            // WERKWIJZE MET LIST
            ticketService = new TicketService();
            IEnumerable<Ticket> tickets = ticketService.GetNietGekoppeldeTicketsList(User.Identity.GetUserId());


            //// ViewModel maken en opvullen
            BezoekerKoppelen bezoekerKoppelen = new BezoekerKoppelen()
            {
                NietGekoppeldeTicketsList = tickets.ToList()
            };


            return View(bezoekerKoppelen);
        }

        [HttpGet]
        public ActionResult Koppel(BezoekerKoppelen bezoekerKoppelen)
        {
            return View(bezoekerKoppelen);
        }

        [HttpPost]
        [ActionName("Koppel")]
        public ActionResult KoppelPost(BezoekerKoppelen bezoekerKoppelen)
        {
            if (ModelState.IsValid)
            {
                // do db stuff
                // redirect
                return RedirectToAction("Index");
            }

            return View(bezoekerKoppelen);
        }



        [HttpPost]
        public ActionResult Index(BezoekerKoppelen bezoekerKoppelenIn)
        {
            if (ModelState.IsValid)
            {
                bezoekerService = new BezoekerService();


                // WERKWIJZE MET LIST
                ticketService = new TicketService();
                IEnumerable<Ticket> tickets = ticketService.GetNietGekoppeldeTicketsList(User.Identity.GetUserId());


                //// ViewModel maken en opvullen
                BezoekerKoppelen bezoekerKoppelen = new BezoekerKoppelen()
                {
                    NietGekoppeldeTicketsList = tickets.ToList()
                };

                // leg de koppeling
                Debug.WriteLine("valid");
            }


   

            return View(bezoekerKoppelenIn);
        }
    }
}