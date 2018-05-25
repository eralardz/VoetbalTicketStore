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

    public class TicketController : BaseController
    {

        private WedstrijdService wedstrijdService;
        private VakService vakService;

        // GET: Ticket
        public ActionResult Buy(int id)
        {
            // Wedstrijd ophalen
            wedstrijdService = new WedstrijdService();
            Wedstrijd wedstrijd = wedstrijdService.GetWedstrijdById(id);

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

        // POST: Confirm
        [HttpPost]
        public ActionResult Confirm(int vakId, int aantalVrijePlaatsen, decimal prijs, int wedstrijdId, int thuisploegId, int bezoekersId, string vakNaam)
        {

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