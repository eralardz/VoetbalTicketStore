using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
        private BestellingService bestellingService;

        // GET: Ticket
        public ActionResult Buy(int id)
        {

            TicketWedstrijd ticketWedstrijd = new TicketWedstrijd();

            wedstrijdService = new WedstrijdService();
            Wedstrijd wedstrijd = wedstrijdService.getWedstrijdById(id);


            ticketWedstrijd.Wedstrijd = wedstrijd;
            ticketWedstrijd.Stadion = wedstrijd.Stadion;
            ticketWedstrijd.Club1 = wedstrijd.Club;
            ticketWedstrijd.Club2 = wedstrijd.Club1;

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
                ticketService = new TicketService();
                bezoekerService = new BezoekerService();
                bestellingService = new BestellingService();

                string user = User.Identity.GetUserId();

                // Bestelling toevoegen indien nodig
                Bestelling bestelling = bestellingService.FindOpenstaandeBestellingDoorUser(user);
                if (bestelling != null)
                {
                    // toevoegen aan bestaande bestelling
                    //TODO
                }
                else
                {
                    // nieuwe bestelling aanmaken
                    bestellingService.CreateNieuweBestelling(0, user);
                }

                // lijn toevoegen aan bestelling

                // Bezoeker toevoegen indien nodig
                if (bezoekerService.FindBezoeker(ticketWedstrijd.Bezoeker.rijksregisternummer) == null)
                {
                    bezoekerService.AddBezoeker(ticketWedstrijd.Bezoeker);
                }

                // Ticket toevoegen
                ticketService.BuyTicket(ticketWedstrijd.SelectedVak, ticketWedstrijd.Stadion.id, ticketWedstrijd.Wedstrijd.id, user, ticketWedstrijd.Bezoeker.rijksregisternummer);



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