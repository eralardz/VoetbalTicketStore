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
    [Authorize]
    public class BestellingController : Controller
    {

        private BestellingService bestellingService;
        private TicketService ticketService;


        // GET: Bestelling
        public ActionResult Index()
        {
            // Lijst met vorige bestellingen
            // Bestellingen die nog niet verlopen zijn -> ticket opvragen via pdf
            // Mogelijkheid om te annuleren, week vooraf (wordt post)

            bestellingService = new BestellingService();
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
            ticketService = new TicketService();
            ticketService.AnnuleerTicket(bestellingVM.TicketId);


            TempData["msg"] = "Uw ticket werd geannuleerd.";
            return RedirectToAction("Index");
        }
    }
}