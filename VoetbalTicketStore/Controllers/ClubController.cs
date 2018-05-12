using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.ViewModel;

namespace VoetbalTicketStore.Controllers
{
    public class ClubController : Controller
    {
        private ClubService clubService;

        // GET: Club
        public ActionResult Index()
        {
            clubService = new ClubService();
            var clubs = clubService.All();

            // ViewModel aanmaken en opvullen
            ClubOverview clubOverview = new ClubOverview()
            {
                Clubs = clubService.All().ToList()
            };


            // we geven de lijst met clubs mee aan de view
            return View(clubOverview);
        }
    }
}