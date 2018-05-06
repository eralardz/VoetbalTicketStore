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
    [Authorize]
    public class ShoppingCartController : Controller
    {

        BestellingService bestellingService;


        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(TicketConfirm ticketConfirm)
        {
            // Nieuwe bestelling aanmaken indien nodig
            bestellingService = new BestellingService();
            Bestelling bestelling = bestellingService.CreateNieuweBestellingIndienNodig(User.Identity.GetUserId());

            // Tickets toevoegen
            // TODO: batch add ?
            for(int i = 0; i < ticketConfirm.AantalTickets; i++)
            {
                Ticket ticket = new Ticket()
                {
                    Gebruikerid = User.Identity.GetUserId(),
                    Prijs = ticketConfirm.Prijs,
                    Vakid = ticketConfirm.VakId,
                    Bevestigd = false,
                    BestellingId = bestelling.Id,
                    Wedstrijdid = ticketConfirm.WedstrijdId
                };

            }

            return RedirectToAction("Index","ShoppingCart");
        }
    }
}