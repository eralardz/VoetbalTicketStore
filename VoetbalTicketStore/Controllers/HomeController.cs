using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
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

        public HomeController()
        {

        }

        public HomeController(IWedstrijdService wedstrijdService, UserManager<ApplicationUser> userManager)
        {
            this.wedstrijdService = wedstrijdService;
            this.userManager = userManager;
        }


        public ViewResult Index()
        {
            if (userManager == null)
            {
                userManager = new UserManager<ApplicationUser>(
               new UserStore<ApplicationUser>(
                   new ApplicationDbContext()));
            }

            // Find user
            var user = userManager.FindByIdAsync(User.Identity.GetUserId());
            
            // get aangeraden wedstrijden

            if (wedstrijdService == null)
            {
                wedstrijdService = new WedstrijdService();
            }

            // viewmodel opvullen
            HomeVM homeVM = new HomeVM();

            if (user != null)
            {
                homeVM.HighlightList = wedstrijdService.GetAanTeRadenWedstrijdenVoorClub(user.Result.FavorietTeam, 3).ToList();
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