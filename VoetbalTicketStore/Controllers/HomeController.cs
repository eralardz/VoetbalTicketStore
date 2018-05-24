using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.Controllers
{
    public class HomeController : Controller
    {

        private WedstrijdService wedstrijdService;

        public ViewResult Index()
        {
            // get favoriet team
            var manager = new UserManager<ApplicationUser>(
               new UserStore<ApplicationUser>(
                   new ApplicationDbContext()));

            // Find user
            var user = manager.FindById(User.Identity.GetUserId());
            // get aangeraden wedstrijden
            wedstrijdService = new WedstrijdService();

            // viewmodel opvullen
            HomeVM homeVM = new HomeVM
            {
                //HighlightList = wedstrijdService.GetAanTeRadenWedstrijdenVoorClub(user.FavorietTeam, 3)
            };

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
    }
}