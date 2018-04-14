using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.Controllers
{
    public class TicketController : Controller
    {

        private TicketService ticketService;
             
        // GET: Ticket
        public ActionResult Buy(int id)
        {
            ViewBag.WedstrijdId = id;
            return View();
        }
    }
}