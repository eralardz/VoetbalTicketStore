using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Exceptions;
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
            shoppingCartDataService.AddToShoppingCart(bestelling.Id, ticketConfirm.Prijs, ticketConfirm.WedstrijdId, ticketConfirm.ThuisploegId, ticketConfirm.BezoekersId, ticketConfirm.AantalTickets, ticketConfirm.VakId, User.Identity.GetUserId());

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
        public ActionResult AdjustAmount(ShoppingCart shoppingCart)
        {
            shoppingCartDataService = new ShoppingCartDataService();
            shoppingCartDataService.AdjustAmount(shoppingCart.SelectedShoppingCartData, shoppingCart.NieuweHoeveelheid, User.Identity.GetUserId(), shoppingCart.GeselecteerdeWedstrijd);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Clear()
        {
            bestellingService = new BestellingService();
            bestellingService.RemoveBestelling(User.Identity.GetUserId());


            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Finalise()
        {
            // Alle ShoppingCartData binnenhalen (via eager bestelling)
            bestellingService = new BestellingService();
            Bestelling bestelling = bestellingService.FindOpenstaandeBestellingDoorUser(User.Identity.GetUserId());

            // Is er nog voldoende plaats in het vak om dit ticket aan te maken?
            ticketService = new TicketService();
            IList<Ticket> tickets = new List<Ticket>();
            foreach (ShoppingCartData shoppingCartData in bestelling.ShoppingCartDatas)
            {
                int totaalAantal = ticketService.GetAantalVerkochteTicketsVoorVak(shoppingCartData.Vak, shoppingCartData.Wedstrijd) + shoppingCartData.Hoeveelheid;

                int rest = shoppingCartData.Vak.MaximumAantalZitplaatsen - totaalAantal;

                if (rest > 0)
                {
                    // tickets mogen aangemaakt worden
                    Ticket ticket = new Ticket();
                    ticket.Gebruikerid = User.Identity.GetUserId();
                    ticket.Prijs = shoppingCartData.Prijs;
                    // nullable attributes
                    if (shoppingCartData.VakId != null || shoppingCartData.WedstrijdId == null)
                    {
                        ticket.Vakid = (int)shoppingCartData.VakId;
                        ticket.Wedstrijdid = (int)shoppingCartData.WedstrijdId;

                    }
                    ticket.BestellingId = shoppingCartData.BestellingId;

                    tickets.Add(ticket);

                }
                else
                {
                    // TODO: eventueel clear shoppingcart?
                    // tickets mogen niet aangemaakt worden
                    throw new BestelException("Tickets reeds uitverkocht! Helaas!");
                }
            }

            // add in bulk
            ticketService.AddTickets(tickets);

            // delete shoppingcartdata
            shoppingCartDataService = new ShoppingCartDataService();
            shoppingCartDataService.RemoveAllShoppingCartData(User.Identity.GetUserId());


            // Geval abonnement

            return View("Index");

        }
    }
}
