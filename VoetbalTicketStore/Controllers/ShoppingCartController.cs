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
            // Openstaande bestelling ophalen
            bestellingService = new BestellingService();
            Bestelling bestelling = bestellingService.FindOpenstaandeBestellingDoorUser(User.Identity.GetUserId());

            // ViewModel aanmaken en opvullen
            ShoppingCart shoppingCart = null;
            if (bestelling != null)
            {
                shoppingCart = new ShoppingCart()
                {
                    Bestelling = bestelling,
                    ShoppingCartEntries = bestelling.ShoppingCartDatas.ToList(),
                    TotaalPrijs = bestellingService.BerekenTotaalPrijs(bestelling.ShoppingCartDatas)
                };
            }
            return View(shoppingCart);
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
            shoppingCartDataService.AddToShoppingCart(bestelling.Id, ticketConfirm.Prijs, ticketConfirm.WedstrijdId, ticketConfirm.AantalTickets, ticketConfirm.VakId, User.Identity.GetUserId());

            // RedirectToAction ipv View, anders wordt geen model meegegeven!
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            shoppingCartDataService = new ShoppingCartDataService();
            shoppingCartDataService.RemoveShoppingCartData(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AdjustAmount(int id, int newAmount)
        {
            shoppingCartDataService = new ShoppingCartDataService();
            shoppingCartDataService.AdjustAmount(id, newAmount);
            return RedirectToAction("Index");
        }
    }
}