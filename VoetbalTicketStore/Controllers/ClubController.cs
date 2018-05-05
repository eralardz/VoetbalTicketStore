using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Service;

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
            // we geven de lijst met clubs mee aan de view
            return View(clubs);
        }
    }
}