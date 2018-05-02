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
    public class ShoppingCartController : Controller
    {

        private ShoppingCartDataService shoppingCartDataService;
        private BestellingService bestellingService;

        // GET: ShoppingCart
        public ActionResult Index()
        {
            shoppingCartDataService = new ShoppingCartDataService();
            bestellingService = new BestellingService();
            Bestelling bestelling = bestellingService.GetBestellingMetTicketsByUser(User.Identity.GetUserId());

            // ViewModel opvullen
            ShoppingCart shoppingCart = new ShoppingCart();
            shoppingCart.Bestelling = bestelling;
            shoppingCart.ShoppingCartItems = bestelling.ShoppingCartDatas;
            shoppingCart.Tickets = bestelling.Tickets;
            shoppingCart.TotaalPrijs = shoppingCartDataService.berekenTotaalPrijs(bestelling);

            return View(shoppingCart);
        }

         //In general, you don’t want to perform an HTTP GET operation when invoking an action that modifies the state of your web application.When performing a delete, you want to perform an HTTP POST, or better yet, an HTTP DELETE operation.
        [HttpPost]
        public ActionResult Remove(int id)
        {
            shoppingCartDataService = new ShoppingCartDataService();
            shoppingCartDataService.RemoveShoppingCartData(id);
            return RedirectToAction("Index");
        }
    }
}
 
 