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
    public class TicketController : Controller
    {

        private TicketService ticketService;
        private WedstrijdService wedstrijdService;
        private BezoekerService bezoekerService;

        // GET: Ticket
        public ActionResult Buy(int id)
        {

            TicketWedstrijd ticketWedstrijd = new TicketWedstrijd();

            wedstrijdService = new WedstrijdService();
            Wedstrijd wedstrijd = wedstrijdService.getWedstrijdById(id);

            ticketWedstrijd.Wedstrijd = wedstrijd;

            return View(ticketWedstrijd);
        }

        // POST: Ticket
        [HttpPost]
        public ActionResult Buy(TicketWedstrijd ticketWedstrijd)
        {
            try
            {
                // TODO: Add insert logic here
                ticketService = new TicketService();

                // Bezoeker-id controleren en kijken of dergelijke bezoeker al bestaat.
                // Niet nodig aangezien het insert statement op een bestaande bezoeker niks doet.

                // EINDE: Bezoeker toevoegen
                Debug.Write(ticketWedstrijd.Bezoeker.email);
                Debug.Write(ticketWedstrijd.Bezoeker.naam);
                Debug.Write(ticketWedstrijd.Bezoeker.voornaam);


                return RedirectToAction("Success");
            }
            catch
            {
                return View("Fail");
            }
        }

        public ActionResult Success()
        {
            return View("Success");
        }
    }
}