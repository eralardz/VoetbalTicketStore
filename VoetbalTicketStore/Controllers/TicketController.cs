using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.Service.Interfaces;
using VoetbalTicketStore.ViewModel;

namespace VoetbalTicketStore.Controllers
{

    public class TicketController : BaseController
    {

        private IWedstrijdService wedstrijdService;
        private IVakService vakService;

        public TicketController()
        {

        }

        public TicketController(IWedstrijdService wedstrijdService, IVakService vakService)
        {
            this.wedstrijdService = wedstrijdService;
            this.vakService = vakService;
        }

        // GET: Ticket
        public ActionResult Buy(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Wedstrijd ophalen
            if (wedstrijdService == null)
            {
                wedstrijdService = new WedstrijdService();
            }
            Wedstrijd wedstrijd = wedstrijdService.GetWedstrijdById(id);

            // Vakken ophalen van stadion

            if (vakService == null)
            {
                vakService = new VakService();
            }

            IEnumerable<Vak> vakken = vakService.GetVakkenInStadion(wedstrijd.Stadionid);

            // Prijzen bepalen per vak (thuisploeg)
            vakService.BerekenPrijzenBijVakken(vakken, wedstrijd.Club);

            // Vrije plaatsen bepalen per vak
            vakService.BerekenAantalVrijePlaatsen(vakken, wedstrijd, wedstrijd.Club);

            // ViewModel maken en opvullen
            TicketWedstrijd ticketWedstrijd = new TicketWedstrijd
            {
                Wedstrijd = wedstrijd,
                Stadion = wedstrijd.Stadion,
                Thuisploeg = wedstrijd.Club,
                Tegenstanders = wedstrijd.Club1,
                VakkenList = vakken
            };

            return View(ticketWedstrijd);
        }

        // POST: Confirm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(int vakId, int aantalVrijePlaatsen, decimal prijs, int wedstrijdId, int thuisploegId, int bezoekersId, string vakNaam)
        {

            if (vakId < 0 || aantalVrijePlaatsen < 0 || prijs < 0 || wedstrijdId < 0 || thuisploegId < 0 || bezoekersId < 0 || vakNaam == null || vakNaam.Equals(""))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            // Lijst voor aantal tickets
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "1", Value = "1", Selected = true });
            list.Add(new SelectListItem { Text = "2", Value = "2" });
            list.Add(new SelectListItem { Text = "3", Value = "3" });
            list.Add(new SelectListItem { Text = "4", Value = "4" });

            // ViewModel maken en opvullen
            TicketConfirm ticketConfirm = new TicketConfirm()
            {
                AantalVrijePlaatsen = aantalVrijePlaatsen,
                Prijs = prijs,
                HoeveelheidTicketsList = list,
                VakId = vakId,
                VakNaam = vakNaam,
                WedstrijdId = wedstrijdId,
                ThuisploegId = thuisploegId,
                BezoekersId = bezoekersId
            };

            return PartialView(ticketConfirm);
        }
    }
}