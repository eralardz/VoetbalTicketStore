using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.ViewModel;

namespace VoetbalTicketStore.Controllers
{
    public class AbonnementController : Controller
    {
        [Authorize]
        // GET: Abonnement
        public ActionResult Buy(int id, string ploegNaam)
        {
            AbonnementBuy viewModel = new AbonnementBuy()
            {
                PloegId = id,
                PloegNaam = ploegNaam
            };
            return View(viewModel);
        }
    }
}