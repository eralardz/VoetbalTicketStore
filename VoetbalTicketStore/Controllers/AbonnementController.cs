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
    public class AbonnementController : Controller
    {

        private VakService vakService;
        private ClubService clubService;

        [Authorize]
        // GET: Abonnement
        public ActionResult Buy(ClubOverview clubOverview)
        {
            vakService = new VakService();
            clubService = new ClubService();

            IList<Vak> thuisVakken = vakService.GetThuisVakkeninStadion(clubOverview.Stadionid).ToList();

            vakService.BerekenAbonnementPrijzenBijVakken(thuisVakken, clubService.GetClub(clubOverview.GekozenClubId));


            // Lijst voor aantal tickets
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "1", Value = "1", Selected = true });
            list.Add(new SelectListItem { Text = "2", Value = "2" });
            list.Add(new SelectListItem { Text = "3", Value = "3" });
            list.Add(new SelectListItem { Text = "4", Value = "4" });

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