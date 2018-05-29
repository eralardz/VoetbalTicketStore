using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Exceptions;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.ViewModel;

namespace VoetbalTicketStore.Controllers
{
    [Authorize]
    public class ShoppingCartController : BaseController
    {

        private IBestellingService bestellingService;
        private IShoppingCartDataService shoppingCartDataService;
        private UserManager<ApplicationUser> userManager;


        public ShoppingCartController()
        {

        }

        public ShoppingCartController(IBestellingService bestellingService, IShoppingCartDataService shoppingCartDataService, UserManager<ApplicationUser> userManager)
        {
            this.bestellingService = bestellingService;
            this.shoppingCartDataService = shoppingCartDataService;
            this.userManager = userManager;
        }

        // GET: ShoppingCart
        public ActionResult Index()
        {
            if (TempData["error"] != null)
            {
                ViewBag.Error = TempData["error"].ToString();
            }
            if (TempData["success"] != null)
            {
                ViewBag.Success = TempData["success"].ToString();
            }

            // Openstaande bestelling ophalen
            if (bestellingService == null)
            {
                bestellingService = new BestellingService();
            }
            Bestelling bestelling = bestellingService.FindOpenstaandeBestellingDoorUser(User.Identity.GetUserId());

            // Lijst voor aantallen
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Text = "1", Value = "1" },
                new SelectListItem { Text = "2", Value = "2" },
                new SelectListItem { Text = "3", Value = "3" },
                new SelectListItem { Text = "4", Value = "4" }
            };

            // ViewModel aanmaken en opvullen
            ShoppingCart shoppingCart = null;
            if (bestelling != null)
            {
                shoppingCart = new ShoppingCart()
                {
                    Bestelling = bestelling,
                    ShoppingCartEntries = bestelling.ShoppingCartDatas.ToList(),
                    TotaalPrijs = bestellingService.BerekenTotaalPrijs(bestelling.ShoppingCartDatas),
                    HoeveelheidList = list
                };
            }
            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(TicketConfirm ticketConfirm)
        {
            if (ticketConfirm == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Nieuwe bestelling aanmaken indien nodig
            Bestelling bestelling = CreateNieuweBestellingIndienNodig();

            // ShoppingCartData toevoegen
            if (shoppingCartDataService == null)
            {
                shoppingCartDataService = new ShoppingCartDataService();
            }
            shoppingCartDataService.AddToShoppingCart(bestelling.Id, ticketConfirm.Prijs, ticketConfirm.WedstrijdId, ticketConfirm.ThuisploegId, ticketConfirm.BezoekersId, ticketConfirm.AantalTickets, ticketConfirm.VakId, User.Identity.GetUserId());

            // Success message meegeven
            SetSuccessfulAddMessage("Uw winkelwagentje werd aangepast!");
            // RedirectToAction ipv View, anders wordt geen model meegegeven!
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAbonnement(AbonnementBuy abonnementBuy)
        {
            if (abonnementBuy == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Nieuwe bestelling aanmaken indien nodig
            Bestelling bestelling = CreateNieuweBestellingIndienNodig();

            // ShoppingCartData toevoegen
            if (shoppingCartDataService == null)
            {
                shoppingCartDataService = new ShoppingCartDataService();
            }
            shoppingCartDataService.AddAbonnementToShoppingCart(bestelling.Id, abonnementBuy.Prijs, abonnementBuy.AantalAbonnementen, abonnementBuy.GeselecteerdVakId, abonnementBuy.PloegId, User.Identity.GetUserId());

            // Success message meegeven
            SetSuccessfulAddMessage("Uw winkelwagentje werd aangepast!");

            return RedirectToAction("Index");
        }

        private void SetSuccessfulAddMessage(string message)
        {
            TempData["success"] = message;
        }

        private Bestelling CreateNieuweBestellingIndienNodig()
        {
            if (bestellingService == null)
            {
                bestellingService = new BestellingService();
            }
            return bestellingService.CreateNieuweBestellingIndienNodig(User.Identity.GetUserId());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (shoppingCartDataService == null)
            {
                shoppingCartDataService = new ShoppingCartDataService();
            }
            shoppingCartDataService.RemoveShoppingCartData(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdjustAmount(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // hoeveelheid aanpassen
            if (shoppingCartDataService == null)
            {
                shoppingCartDataService = new ShoppingCartDataService();
            }
            shoppingCartDataService.AdjustAmount(shoppingCart.SelectedShoppingCartData, shoppingCart.NieuweHoeveelheid, User.Identity.GetUserId(), shoppingCart.GeselecteerdeWedstrijd);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Clear(ShoppingCart shoppingCart)
        {

            if (shoppingCart == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (shoppingCartDataService == null)
            {
                shoppingCartDataService = new ShoppingCartDataService();
            }
            // Bestelling deleten en dan de bestellijnen cascaden zou handig zijn, maar lukt niet aangezien de FK constraint voor de tickets dan overtreden wordt
            shoppingCartDataService.RemoveShoppingCartDataVanBestelling(shoppingCart.GeselecteerdeBestelling);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Finalise(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                // Plaats bestelling
                if (bestellingService == null)
                {
                    bestellingService = new BestellingService();
                }
                Bestelling bestelling = bestellingService.FindOpenstaandeBestellingDoorUser(User.Identity.GetUserId());
                bestellingService.PlaatsBestelling(bestelling, User.Identity.GetUserId());

                // Find & update user (meest favoriete team)

                if (userManager == null)
                {
                    userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(
                        new ApplicationDbContext()));
                }
                var user = userManager.FindById(User.Identity.GetUserId());
                user.FavorietTeam = bestellingService.GetMeestGekochteThuisploeg(User.Identity.GetUserId());
                userManager.Update(user);
            }
            catch (BestelException ex)
            {
                // Teveel tickets
                // Geen plaats meer

                // When you use redirection, you shall not use ViewBag, but TempData
                // TempData passes data between the current and next HTTP request
                TempData["error"] = ex.Message;
                return RedirectToAction("Index");
            }

            TempData["msg"] = "Uw bestelling werd succesvol afgerond!";
            return RedirectToAction("Index", "Bezoeker");
        }
    }
}
