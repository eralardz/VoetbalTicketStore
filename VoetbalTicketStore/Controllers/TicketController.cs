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
        private VakTypeService vakTypeService;

        // GET: Ticket
        public ActionResult Buy(int id)
        {

            TicketWedstrijd ticketWedstrijd = new TicketWedstrijd();

            wedstrijdService = new WedstrijdService();
            Wedstrijd wedstrijd = wedstrijdService.getWedstrijdById(id);

            ticketWedstrijd.Wedstrijd = wedstrijd;

            vakTypeService = new VakTypeService();
            // id = datavalue (modelwaarde), beschrijving = datatextfield (uitzicht in de view)
            ticketWedstrijd.Vakken = new SelectList(vakTypeService.All(), "id", "beschrijving");


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
                bezoekerService = new BezoekerService();

                // Bezoeker-id controleren en kijken of dergelijke bezoeker al bestaat.
                // Niet nodig aangezien het insert statement op een bestaande bezoeker niks doet. OF ZO DACHT IK... gooit geen Exception in Java maar wel in MSSQL.

                // Bezoeker toevoegen indien nodig
                if(bezoekerService.FindBezoeker(ticketWedstrijd.Bezoeker.rijksregisternummer) == null)
                {
                    bezoekerService.AddBezoeker(ticketWedstrijd.Bezoeker);
                }

                // Ticket toevoegen
                ticketService.BuyTicket(ticketWedstrijd.Ticket, ticketWedstrijd.SelectedVak);



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