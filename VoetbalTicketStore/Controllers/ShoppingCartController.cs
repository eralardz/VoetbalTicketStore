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

        private BestellingService bestellingService;
        private TicketService ticketService;
        private ShoppingCartDataService shoppingCartDataService;

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

            // Tickets toevoegen - MOET EIGENLIJK IN DE TICKETSERVICE TOCH
            // TODO: batch add ?
            //ticketService = new TicketService();
            //for(int i = 0; i < ticketConfirm.AantalTickets; i++)
            //{
            //    Ticket ticket = new Ticket()
            //    {
            //        Gebruikerid = User.Identity.GetUserId(),
            //        Prijs = ticketConfirm.Prijs,
            //        Vakid = ticketConfirm.VakId,
            //        Bevestigd = false,
            //        BestellingId = bestelling.Id,
            //        Wedstrijdid = ticketConfirm.WedstrijdId
            //    };
            //    ticketService.AddTicket(ticket);
            //}

            // ShoppingCartData toevoegen
            shoppingCartDataService = new ShoppingCartDataService();
            shoppingCartDataService.AddToShoppingCart(bestelling.Id, ticketConfirm.Prijs, ticketConfirm.WedstrijdId, ticketConfirm.AantalTickets, ticketConfirm.VakId);

            return RedirectToAction("Index","ShoppingCart");
        }
    }
}