using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.Service.Interfaces;

namespace VoetbalTicketStore.Controllers
{
    public class StadionController : BaseController
    {
        private IStadionService stadionService;

        public StadionController()
        {

        }

        public StadionController(IStadionService stadionService)
        {
            this.stadionService = stadionService;
        }

        // GET: Stadion
        public ActionResult Index()
        {
            if (stadionService == null)
            {
                stadionService = new StadionService();
            }
            IEnumerable<Stadion> stadia = stadionService.All();

            return View(stadia);
        }
    }
}