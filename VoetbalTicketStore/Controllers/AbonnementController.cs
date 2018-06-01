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
    public class AbonnementController : BaseController
    {


        private IVakService vakService;
        private IClubService clubService;

        public AbonnementController()
        {

        }

        public AbonnementController(IVakService vakService, IClubService clubService)
        {
            this.vakService = vakService;
            this.clubService = clubService;
        }

        [Authorize]
        // GET: Abonnement
        public ActionResult Buy(ClubOverview clubOverview)
        {
            if (clubOverview == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (vakService == null)
            {
                vakService = new VakService();
            }
            if (clubService == null)
            {
                clubService = new ClubService();
            }

            IList<Vak> thuisVakken = vakService.GetThuisVakkeninStadion(clubOverview.Stadionid).ToList();

            vakService.BerekenAbonnementPrijzenBijVakken(thuisVakken, clubService.GetClub(clubOverview.GekozenClubId));

            // Lijst voor aantal tickets
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Text = "1", Value = "1", Selected = true },
                new SelectListItem { Text = "2", Value = "2" },
                new SelectListItem { Text = "3", Value = "3" },
                new SelectListItem { Text = "4", Value = "4" }
            };

            // ViewModel aanmaken en opvullen
            AbonnementBuy abonnementBuy = new AbonnementBuy()
            {
                PloegId = clubOverview.GekozenClubId,
                PloegNaam = clubOverview.GekozenClubNaam,
                Logo = clubOverview.Logo,
                Stadionid = clubOverview.Stadionid,
                StadionNaam = clubOverview.StadionNaam,
                Vakken = thuisVakken,
                HoeveelheidAbonnementenList = list

            };
            return View(abonnementBuy);
        }
    }
}