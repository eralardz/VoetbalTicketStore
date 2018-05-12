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
        private AbonnementService abonnementService;

        // GET: ShoppingCart
        public ActionResult Index()
        {
            if (TempData["error"] != null)
            {
                ViewBag.Exception = TempData["error"].ToString();
            }

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
            Bestelling bestelling = CreateNieuweBestellingIndienNodig();

            // ShoppingCartData toevoegen
            shoppingCartDataService = new ShoppingCartDataService();
            shoppingCartDataService.AddToShoppingCart(bestelling.Id, ticketConfirm.Prijs, ticketConfirm.WedstrijdId, ticketConfirm.ThuisploegId, ticketConfirm.BezoekersId, ticketConfirm.AantalTickets, ticketConfirm.VakId, User.Identity.GetUserId());

            // RedirectToAction ipv View, anders wordt geen model meegegeven!
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddAbonnement(AbonnementBuy abonnementBuy)
        {
            // Nieuwe bestelling aanmaken indien nodig
            Bestelling bestelling = CreateNieuweBestellingIndienNodig();

            // ShoppingCartData toevoegen
            shoppingCartDataService = new ShoppingCartDataService();
            shoppingCartDataService.AddAbonnementToShoppingCart(bestelling.Id, abonnementBuy.Prijs, abonnementBuy.AantalAbonnementen, abonnementBuy.GeselecteerdVakId, abonnementBuy.PloegId, User.Identity.GetUserId());

            return RedirectToAction("Index");
        }

        private Bestelling CreateNieuweBestellingIndienNodig()
        {
            bestellingService = new BestellingService();
            return bestellingService.CreateNieuweBestellingIndienNodig(User.Identity.GetUserId());
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
        public ActionResult Clear(ShoppingCart shoppingCart)
        {
            shoppingCartDataService = new ShoppingCartDataService();
            // Bestelling deleten en dan de bestellijnen cascaden zou handig zijn, maar lukt niet aangezien de FK constraint voor de tickets dan overtreden wordt
            shoppingCartDataService.RemoveShoppingCartDataVanBestelling(shoppingCart.GeselecteerdeBestelling);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Finalise(ShoppingCart shoppingCart)
        {
            try
            {
                // Alle ShoppingCartData binnenhalen (via eager bestelling)
                bestellingService = new BestellingService();
                Bestelling bestelling = bestellingService.FindOpenstaandeBestellingDoorUser(User.Identity.GetUserId());

                IList<Ticket> tickets = new List<Ticket>();
                IList<Abonnement> abonnementen = new List<Abonnement>();

                ticketService = new TicketService();
                abonnementService = new AbonnementService();

                foreach (ShoppingCartData shoppingCartData in bestelling.ShoppingCartDatas)
                {
                    // geval ticket
                    // TODO: enums aanmaken
                    if (shoppingCartData.ShoppingCartDataTypeId == 1)
                    {
                        // Is er nog voldoende plaats in het vak om dit ticket aan te maken?
                        int totaalAantal = ticketService.GetAantalVerkochteTicketsVoorVak(shoppingCartData.Vak, shoppingCartData.Wedstrijd) + shoppingCartData.Hoeveelheid;

                        int rest = shoppingCartData.Vak.MaximumAantalZitplaatsen - totaalAantal;

                        if (rest > 0)
                        {
                            // tickets mogen aangemaakt worden
                            Ticket ticket = new Ticket();
                            ticket.Gebruikerid = User.Identity.GetUserId();
                            ticket.Prijs = shoppingCartData.Prijs;
                            ticket.Vakid = shoppingCartData.VakId;

                            // nullable attributes
                            if (shoppingCartData.WedstrijdId != null)
                            {
                                ticket.Wedstrijdid = (int)shoppingCartData.WedstrijdId;

                            }
                            ticket.BestellingId = shoppingCartData.BestellingId;

                            tickets.Add(ticket);
                        }
                    }
                    // geval abonnement
                    else if(shoppingCartData.ShoppingCartDataTypeId == 2) {
                        Abonnement abonnement = new Abonnement()
                        {
                            Clubid = shoppingCartData.Thuisploeg,
                            Prijs = shoppingCartData.Prijs,
                            VakTypeId = shoppingCartData.VakId
                        };

                        abonnementen.Add(abonnement);
                    }
                }

                // add in bulk
                ticketService.AddTickets(tickets);
                abonnementService.AddAbonnementen(abonnementen);

                // bestelling bevestigen
                bestellingService.BevestigBestelling(bestelling.Id);

                // delete shoppingcartdata
                shoppingCartDataService = new ShoppingCartDataService();
                shoppingCartDataService.RemoveShoppingCartDataVanBestelling(bestelling.Id);
            }
            catch (BestelException ex)
            {
                // Teveel tickets
                // Geen plaats meer

                // When you use redirection, you shall not use ViewBag, but TempData
                // TempData passes data between the current and next HTTP request
                TempData["error"] = ex.Message;
            }

            return RedirectToAction("Index","Bezoeker");
        }
    }
}
