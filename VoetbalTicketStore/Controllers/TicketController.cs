﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.ViewModel;

namespace VoetbalTicketStore.Controllers
{

    public class TicketController : Controller
    {

        private WedstrijdService wedstrijdService;
        private VakService vakService;

        // GET: Ticket
        public ActionResult Buy(int id)
        {
            // Wedstrijd ophalen
            wedstrijdService = new WedstrijdService();
            Wedstrijd wedstrijd = wedstrijdService.getWedstrijdById(id);

            // Vakken ophalen van stadion
            vakService = new VakService();
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

        // GET: Ticket
        public ActionResult Confirm(int vakId, int stadionId, int wedstrijdId, int thuisploegId, int tegenstandersId, string naamThuisploeg)
        {
            ViewBag.NaamThuisPloeg = naamThuisploeg;
            return View("Confirm");
        }

    }
}