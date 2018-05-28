using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.ViewModel;

namespace VoetbalTicketStore.Controllers
{
    [Authorize]
    public class BestellingController : BaseController
    {

        private BestellingService bestellingService;
        private ITicketService ticketService;

        public BestellingController(ITicketService ticketService, BestellingService bestellingService)
        {
            this.bestellingService = bestellingService;
            this.ticketService = ticketService;
        }

        public BestellingController()
        {
            bestellingService = new BestellingService();
            ticketService = new TicketService();
        }

        // GET: Bestelling
        public ActionResult Index()
        {
            //bestellingService = new BestellingService();
            IEnumerable<Bestelling> bestellingen = bestellingService.All(User.Identity.GetUserId());

            BestellingVM bestellingVM = new BestellingVM()
            {
                Bestellingen = bestellingen
            };

            if (TempData["msg"] != null)
            {
                ViewBag.Msg = TempData["msg"].ToString();
            }

            return View(bestellingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Annuleren(BestellingVM bestellingVM)
        {
            if (bestellingVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //ticketService = new TicketService();
            ticketService.AnnuleerTicket(bestellingVM.TicketId);

            TempData["msg"] = "Uw ticket werd geannuleerd.";
            return RedirectToAction("Index");
        }
    }
}