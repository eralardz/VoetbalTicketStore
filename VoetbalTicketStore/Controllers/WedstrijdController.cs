using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.Controllers
{
    public class WedstrijdController : BaseController
    {

        IWedstrijdService wedstrijdService;


        public WedstrijdController()
        {

        }

        public WedstrijdController(IWedstrijdService wedstrijdService)
        {
            this.wedstrijdService = wedstrijdService;
        }



        // GET: Wedstrijd
        public ActionResult Index()
        {
            if(wedstrijdService == null)
            {
                wedstrijdService = new WedstrijdService();
            }
            var wedstrijden = wedstrijdService.GetUpcomingWedstrijden();
            // we geven de lijst met wedstrijden mee aan de view
            return View(wedstrijden);
        }

        public ActionResult WedstrijdKalender(Club club)
        {
            if (club == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (wedstrijdService == null)
            {
                wedstrijdService = new WedstrijdService();
            }
            var wedstrijden = wedstrijdService.GetWedstrijdKalenderVanPloeg(club);
            return View(wedstrijden);
        }
    }
}