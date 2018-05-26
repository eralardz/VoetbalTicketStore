using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.Controllers
{
    public class StadionController : BaseController
    {

        private StadionService stadionService;
        // GET: Stadion
        public ActionResult Index()
        {
            stadionService = new StadionService();
            IEnumerable<Stadion> stadia = stadionService.All();

            return View(stadia);
        }
    }
}