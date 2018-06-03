using Microsoft.AspNet.Identity;
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
    public class BestellingController : BaseController
    {

        private IBestellingService bestellingService;
        private ITicketService ticketService;

        public BestellingController(ITicketService ticketService, IBestellingService bestellingService)
        {
            this.bestellingService = bestellingService;
            this.ticketService = ticketService;
        }

        public BestellingController()
        {

        }

        // GET: Bestelling
        public ActionResult Index()
        {
            if (bestellingService == null)
            {
                bestellingService = new BestellingService();
            }
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

            if (ticketService == null)
            {
                ticketService = new TicketService();
            }

            try
            {
                //ticketService = new TicketService();
                ticketService.AnnuleerTicket(bestellingVM.TicketId);

                TempData["msg"] = "Uw ticket werd geannuleerd.";
                return RedirectToAction("Index");
            }
            catch (BestelException ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}