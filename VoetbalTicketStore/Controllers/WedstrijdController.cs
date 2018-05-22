using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.Controllers
{
    public class WedstrijdController : Controller
    {

        WedstrijdService wedstrijdService;

        // GET: Wedstrijd
        public ActionResult Index()
        {
            wedstrijdService = new WedstrijdService();
            var wedstrijden = wedstrijdService.GetUpcomingWedstrijden();
            // we geven de lijst met wedstrijden mee aan de view
            return View(wedstrijden);
        }

        public ActionResult WedstrijdKalender(Club club)
        {
            wedstrijdService = new WedstrijdService();
            var wedstrijden = wedstrijdService.GetWedstrijdKalenderVanPloeg(club);
            return View(wedstrijden);
        }
    }
}