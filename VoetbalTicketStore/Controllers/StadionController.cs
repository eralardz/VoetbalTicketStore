using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.Controllers
{
    public class StadionController : Controller
    {

        private StadionService stadionService;

        // GET: Stadion
        public ActionResult Index()
        {
            stadionService = new StadionService();
            var stadions = stadionService.All();
            // we geven de lijst stadions mee aan de view
            return View(stadions);
        }
    }
}