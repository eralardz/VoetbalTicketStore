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

            return View(bestellingVM);
        }
    }
}