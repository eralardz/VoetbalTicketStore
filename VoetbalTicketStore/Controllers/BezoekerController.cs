using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.ViewModel;

namespace VoetbalTicketStore.Controllers
{
    [Authorize]
    public class BezoekerController : BaseController
    {

        private IBezoekerService bezoekerService;
        private IBestellingService bestellingService;
        private ITicketService ticketService;
        private IAbonnementService abonnementService;
        private IPDFService pdfService;
        private IMailService mailService;

        // GET: Bezoeker
        //public ActionResult Index()
        //{
        //    bezoekerService = new BezoekerService();
        //    bestellingService = new BestellingService();

        //    // WERKWIJZE MET LIST
        //    ticketService = new TicketService();
        //    IEnumerable<Ticket> tickets = ticketService.GetNietGekoppeldeTicketsList(User.Identity.GetUserId());


        //    // ViewModel maken en opvullen
        //    BezoekerKoppelen bezoekerKoppelen = new BezoekerKoppelen()
        //    {
        //        NietGekoppeldeTicketsList = tickets.ToList()
        //    };

        //    // Status message
        //    if (TempData["msg"] != null)
        //    {
        //        ViewBag.Msg = TempData["msg"].ToString();
        //    }

        //    return View(bezoekerKoppelen);
        //}

        public BezoekerController()
        {

        }

        // Gebruikt voor tests
        public BezoekerController(IBezoekerService bezoekerService, IBestellingService bestellingService, ITicketService ticketService, IAbonnementService abonnementService, IPDFService pdfService, IMailService mailService)
        {
            this.bezoekerService = bezoekerService;
            this.bestellingService = bestellingService;
            this.ticketService = ticketService;
            this.abonnementService = abonnementService;
            this.pdfService = pdfService;
            this.mailService = mailService;
        }


        public ActionResult Index()
        {
            // tickets
            ticketService = new TicketService();
            IEnumerable<IGrouping<Bestelling, Ticket>> tickets = ticketService.GetNietGekoppeldeTickets(User.Identity.GetUserId());

            // abonnementen
            abonnementService = new AbonnementService();
            IEnumerable<Abonnement> abonnementen = abonnementService.GetNietGekoppeldeAbonnementen(User.Identity.GetUserId());

            BezoekerKoppelen bezoekerKoppelen = new BezoekerKoppelen()
            {
                NietGekoppeldeTickets = tickets,
                NietGekoppeldeAbonnementen = abonnementen
            };

            // Status message
            if (TempData["msg"] != null)
            {
                ViewBag.Msg = TempData["msg"].ToString();
            }

            return View(bezoekerKoppelen);
        }

        [HttpGet]
        public ActionResult Koppel(BezoekerKoppelen bezoekerKoppelen)
        {
            if (bezoekerKoppelen == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Text = "Mezelf", Value = "0", Selected = true },
                new SelectListItem { Text = "Iemand anders", Value = "1" },
            };

            // ViewModel opvullen
            bezoekerKoppelen.TypeBezoekerList = list;

            // Create manager
            var manager = new UserManager<ApplicationUser>(
               new UserStore<ApplicationUser>(
                   new ApplicationDbContext()));

            // Find user
            var user = manager.FindById(User.Identity.GetUserId());

            bezoekerKoppelen.ActieveGebruikerVoornaam = user.FirstName;
            bezoekerKoppelen.ActieveGebruikerFamilienaam = user.LastName;
            bezoekerKoppelen.ActieveGebruikerEmail = user.Email;
            bezoekerKoppelen.ActieveGebruikerRijksregisternummer = user.Rijksregisternummer;

            return View(bezoekerKoppelen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Koppel")]
        public async Task<ActionResult> KoppelPostAsync(BezoekerKoppelen bezoekerKoppelen)
        {
            if (bezoekerKoppelen == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                if (bezoekerService == null)
                {
                    bezoekerService = new BezoekerService();
                }
                // bezoeker aanmaken indien nodig
                Bezoeker bezoeker = bezoekerService.AddBezoekerIndienNodig(bezoekerKoppelen.TeWijzigenBezoeker.Rijksregisternummer, bezoekerKoppelen.TeWijzigenBezoeker.Naam, bezoekerKoppelen.TeWijzigenBezoeker.Voornaam, bezoekerKoppelen.TeWijzigenBezoeker.Email);

                if (mailService == null)
                {
                    mailService = new MailService();
                }

                MailMessage message = mailService.GenerateMail(bezoekerKoppelen.TeWijzigenBezoeker.Email, bezoekerKoppelen.TeWijzigenBezoeker.Voornaam);

                // geval ticket
                if (bezoekerKoppelen.TeWijzigenTicket != 0)
                {
                    if (ticketService == null)
                    {
                        ticketService = new TicketService();
                    }
                    // koppelen
                    ticketService.KoppelBezoekerAanTicket(bezoekerKoppelen.TeWijzigenTicket, bezoeker.Rijksregisternummer);

                    // ticket + alle info nodig om voucher te genereren
                    Ticket ticket = ticketService.FindTicket(bezoekerKoppelen.TeWijzigenTicket);

                    if (pdfService == null)
                    {
                        pdfService = new PDFService();
                    }
                    pdfService.setPDFInfo(true, ticket.Id, ticket.BestellingId, ticket.Prijs, ticket.Wedstrijd.Club.Naam, ticket.Wedstrijd.Club1.Naam, ticket.Wedstrijd.Stadion.Adres, null, ticket.Wedstrijd.DatumEnTijd, ticket.Bezoeker.Voornaam, ticket.Bezoeker.Naam, ticket.Bezoekerrijksregisternummer, ticket.Bezoeker.Email);

                    message.Attachments.Add(pdfService.GetAttachment());
                }

                // geval abonnement
                else
                {
                    if (abonnementService == null)
                    {
                        abonnementService = new AbonnementService();
                    }
                    abonnementService.KoppelBezoekerAanAbonnement(bezoekerKoppelen.TeWijzigenAbonnement, bezoeker.Rijksregisternummer);

                    Abonnement abonnement = abonnementService.FindAbonnement(bezoekerKoppelen.TeWijzigenAbonnement);

                    if (pdfService == null)
                    {
                        pdfService = new PDFService();
                    }
                    pdfService.setPDFInfo(false, abonnement.Id, abonnement.BestellingId, abonnement.Prijs, abonnement.Club.Naam, null, null, abonnement.Club.Stadion.Naam, abonnement.Bestelling.BestelDatum, abonnement.Bezoeker.Voornaam, abonnement.Bezoeker.Naam, abonnement.Bezoekerrijksregisternummer, abonnement.Bezoeker.Email);

                    message.Attachments.Add(pdfService.GetAttachment());
                }


                await mailService.SendMailAsync(message);

                TempData["msg"] = "Uw ticket werd bevestigd. De voucher werd verstuurd naar " +
                    bezoekerKoppelen.TeWijzigenBezoeker.Email + ". De voucher is ook beschikbaar op deze website.";
                // redirect
                return RedirectToAction("Index");
            }
            return View(bezoekerKoppelen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateTicketPDF(BestellingVM bestellingVM)
        {
            if (bestellingVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (pdfService == null)
            {
                pdfService = new PDFService();
            }

            pdfService.setPDFInfo(true, bestellingVM.TicketId, bestellingVM.BestellingId, bestellingVM.Prijs, bestellingVM.ThuisploegNaam, bestellingVM.TegenstandersNaam, bestellingVM.StadionAdres, bestellingVM.StadionNaam, bestellingVM.WedstrijdDatumEnTijd, bestellingVM.BezoekerVoornaam, bestellingVM.BezoekerNaam, bestellingVM.BezoekerRijksregisternummer, bestellingVM.BezoekerEmail);


            Byte[] bytes = pdfService.ConvertHtmlToPDF();
            // openen in browser
            return File(bytes, "application/pdf", "voucher.pdf");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateAbonnementPDF(BestellingVM bestellingVM)
        {
            if (bestellingVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (pdfService == null)
            {
                pdfService = new PDFService();
            }

            pdfService.setPDFInfo(false, bestellingVM.AbonnementId, bestellingVM.BestellingId, bestellingVM.Prijs, bestellingVM.ThuisploegNaam, null, null, bestellingVM.StadionNaam, bestellingVM.BestelDatum, bestellingVM.BezoekerVoornaam, bestellingVM.BezoekerNaam, bestellingVM.BezoekerRijksregisternummer, bestellingVM.BezoekerEmail);

            Byte[] bytes = pdfService.ConvertHtmlToPDF();
            // openen in browser
            return File(bytes, "application/pdf", "abonnement.pdf");
        }

        [HttpPost]
        public ActionResult Index(BezoekerKoppelen bezoekerKoppelenIn)
        {
            if (ModelState.IsValid)
            {
                if (bezoekerService == null)
                {
                    bezoekerService = new BezoekerService();
                }

                // WERKWIJZE MET LIST
                ticketService = new TicketService();
                IEnumerable<Ticket> tickets = ticketService.GetNietGekoppeldeTicketsList(User.Identity.GetUserId());


                //// ViewModel maken en opvullen
                BezoekerKoppelen bezoekerKoppelen = new BezoekerKoppelen()
                {
                    NietGekoppeldeTicketsList = tickets.ToList()
                };
            }
            return View(bezoekerKoppelenIn);
        }
    }
}