using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Helpers;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.ViewModel;

namespace VoetbalTicketStore.Controllers
{
    public class HomeController : BaseController
    {

        private IWedstrijdService wedstrijdService;
        private UserManager<ApplicationUser> userManager;
        private IBestellingService bestellingService;

        public HomeController()
        {

        }

        public HomeController(IWedstrijdService wedstrijdService, UserManager<ApplicationUser> userManager, IBestellingService bestellingService)
        {
            this.wedstrijdService = wedstrijdService;
            this.userManager = userManager;
            this.bestellingService = bestellingService;
        }


        public async Task<ViewResult> Index()
        {
            if (userManager == null)
            {
                userManager = new UserManager<ApplicationUser>(
               new UserStore<ApplicationUser>(
                   new ApplicationDbContext()));
            }

            // Find user
            var user = await userManager.FindByIdAsync(User.Identity.GetUserId());

            // get aangeraden wedstrijden

            if (wedstrijdService == null)
            {
                wedstrijdService = new WedstrijdService();
            }

            // viewmodel opvullen
            HomeVM homeVM = new HomeVM();

            if (user != null)
            {
                if (Session["ShoppingCartItemTotal"] == null)
                {
                    if (bestellingService == null)
                    {
                        bestellingService = new BestellingService();
                        Bestelling bestelling = bestellingService.FindOpenstaandeBestellingDoorUser(user.Id);
                        if (bestelling != null)
                        {
                            int totaalAantalitems = 0;
                            foreach (ShoppingCartData s in bestelling.ShoppingCartDatas)
                            {
                                totaalAantalitems += s.Hoeveelheid;
                            }
                            if (totaalAantalitems > 0)
                            {
                                Session["ShoppingCartItemTotal"] = totaalAantalitems;
                            }
                            else
                            {
                                Session["ShoppingCartItemTotal"] = null;
                            }
                        }
                    }
                }

                // opgehaalde lijst verandert normaal niet snel, wordt tijdelijk in de session opgeslagen om het aantal SQL-queries te beperken
                List<Wedstrijd> list = (List<Wedstrijd>)Session["AanTeRadenWedstrijden"];
                if (list == null)
                {
                    homeVM.HighlightList = wedstrijdService.GetAanTeRadenWedstrijdenVoorClub(user.FavorietTeam, 3).ToList();
                    Session["AanTeRadenWedstrijden"] = homeVM.HighlightList;
                }
                else
                {
                    homeVM.HighlightList = list;
                }
            }

            return View(homeVM);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // Culture-cookie instellen
        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}