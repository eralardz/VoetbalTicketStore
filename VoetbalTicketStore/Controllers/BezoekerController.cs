using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
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

        private BezoekerService bezoekerService;
        private BestellingService bestellingService;
        private TicketService ticketService;
        private AbonnementService abonnementService;

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
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Text = "Mezelf", Value = "0", Selected = true },
                new SelectListItem { Text = "Iemand anders", Value = "1" },
            };

            // ViewModel opvullen
            bezoekerKoppelen.TypeBezoekerList = list;

            // 


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
        public async System.Threading.Tasks.Task<ActionResult> KoppelPostAsync(BezoekerKoppelen bezoekerKoppelen)
        {
            if (ModelState.IsValid)
            {
                bezoekerService = new BezoekerService();
                // bezoeker aanmaken indien nodig
                Bezoeker bezoeker = bezoekerService.AddBezoekerIndienNodig(bezoekerKoppelen.TeWijzigenBezoeker.Rijksregisternummer, bezoekerKoppelen.TeWijzigenBezoeker.Naam, bezoekerKoppelen.TeWijzigenBezoeker.Voornaam, bezoekerKoppelen.TeWijzigenBezoeker.Email);

                // gegevens mail
                var message = new MailMessage();
                message.To.Add(new MailAddress(bezoekerKoppelen.TeWijzigenBezoeker.Email));
                message.From = new MailAddress("vbtstore2018@gmail.com");
                message.Subject = "Uw bestelling bij VoetbalTicketStore";
                var body = "<p>Bedankt voor uw bestelling!: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                message.Body = string.Format(body, bezoekerKoppelen.TeWijzigenBezoeker.Naam, bezoekerKoppelen.TeWijzigenBezoeker.Email, "bla");
                message.IsBodyHtml = true;

                // GEVAL TICKET
                if (bezoekerKoppelen.TeWijzigenTicket != 0)
                {
                    // rijksregisternummer aan ticket koppelen
                    ticketService = new TicketService();
                    ticketService.KoppelBezoekerAanTicket(bezoekerKoppelen.TeWijzigenTicket, bezoeker.Rijksregisternummer);

                    // ticket + alle info nodig om voucher te genereren
                    Ticket ticket = ticketService.FindTicket(bezoekerKoppelen.TeWijzigenTicket);

                    TicketPDF ticketPDF = new TicketPDF()
                    {
                        TicketId = ticket.Id,
                        BestellingId = ticket.BestellingId,
                        Prijs = ticket.Prijs,
                        ThuisploegNaam = ticket.Wedstrijd.Club.Naam,
                        TegenstandersNaam = ticket.Wedstrijd.Club1.Naam,
                        StadionNaam = ticket.Wedstrijd.Stadion.Naam,
                        StadionAdres = ticket.Wedstrijd.Stadion.Adres,
                        WedstrijdDatumEnTijd = ticket.Wedstrijd.DatumEnTijd,
                        BezoekerVoornaam = ticket.Bezoeker.Voornaam,
                        BezoekerNaam = ticket.Bezoeker.Naam,
                        BezoekerRijksregisternummer = ticket.Bezoekerrijksregisternummer,
                        BezoekerEmail = ticket.Bezoeker.Email
                    };

                    message.Attachments.Add(GetAttachment(ticketPDF, null));
                }

                // GEVAL ABONNEMENT
                else
                {
                    abonnementService = new AbonnementService();
                    abonnementService.KoppelBezoekerAanAbonnement(bezoekerKoppelen.TeWijzigenAbonnement, bezoeker.Rijksregisternummer);

                    Abonnement abonnement = abonnementService.FindAbonnement(bezoekerKoppelen.TeWijzigenAbonnement);

                    AbonnementPDF abonnementPDF = new AbonnementPDF()
                    {
                        AbonnementId = abonnement.Id,
                        BestellingId = abonnement.BestellingId,
                        Prijs = abonnement.Prijs,
                        ClubNaam = abonnement.Club.Naam,
                        StadionNaam = abonnement.Club.Stadion.Naam,
                        SeizoenJaar = abonnement.Bestelling.BestelDatum.Year,
                        BezoekerVoornaam = abonnement.Bezoeker.Voornaam,
                        BezoekerNaam = abonnement.Bezoeker.Naam,
                        BezoekerRijksregisternummer = abonnement.Bezoekerrijksregisternummer,
                        BezoekerEmail = abonnement.Bezoeker.Email
                    };

                    message.Attachments.Add(GetAttachment(null, abonnementPDF));
                }


                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                }

                TempData["msg"] = "Uw ticket werd bevestigd. De voucher werd verstuurd naar " +
                    bezoekerKoppelen.TeWijzigenBezoeker.Email + ". De voucher is ook beschikbaar op deze website.";
                // redirect
                return RedirectToAction("Index");
            }
            return View(bezoekerKoppelen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileContentResult GenerateTicketPDF(BestellingVM bestellingVM)
        {
            TicketPDF ticketPDF = new TicketPDF()
            {
                TicketId = bestellingVM.TicketId,
                BestellingId = bestellingVM.BestellingId,
                Prijs = bestellingVM.Prijs,
                ThuisploegNaam = bestellingVM.ThuisploegNaam,
                TegenstandersNaam = bestellingVM.TegenstandersNaam,
                StadionNaam = bestellingVM.StadionNaam,
                StadionAdres = bestellingVM.StadionAdres,
                WedstrijdDatumEnTijd = bestellingVM.WedstrijdDatumEnTijd,
                BezoekerVoornaam = bestellingVM.BezoekerVoornaam,
                BezoekerNaam = bestellingVM.BezoekerNaam,
                BezoekerRijksregisternummer = bestellingVM.BezoekerRijksregisternummer,
                BezoekerEmail = bestellingVM.BezoekerEmail
            };


            Byte[] bytes = ConvertHtmlToPDF(ticketPDF, null);
            // openen in browser
            return File(bytes, "application/pdf", "voucher.pdf");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileContentResult GenerateAbonnementPDF(BestellingVM bestellingVM)
        {
            AbonnementPDF abonnementPDF = new AbonnementPDF()
            {
                AbonnementId = bestellingVM.AbonnementId,
                BestellingId = bestellingVM.BestellingId,
                Prijs = bestellingVM.Prijs,
                ClubNaam = bestellingVM.ThuisploegNaam,
                StadionNaam = bestellingVM.StadionNaam,
                SeizoenJaar = bestellingVM.BestelDatum.Year,
                BezoekerVoornaam = bestellingVM.BezoekerVoornaam,
                BezoekerNaam = bestellingVM.BezoekerNaam,
                BezoekerRijksregisternummer = bestellingVM.BezoekerRijksregisternummer,
                BezoekerEmail = bestellingVM.BezoekerEmail
            };


            Byte[] bytes = ConvertHtmlToPDF(null, abonnementPDF);
            // openen in browser
            return File(bytes, "application/pdf", "abonnement.pdf");
        }


        // This tool parses (X)HTML snippets and the associated CSS and converts them to PDF.
        // XMLWorker is an extra component for iText®. The first XML to PDF implementation, is a new version of the old HTMLWorker that used to be shipped with iText.
        private Byte[] ConvertHtmlToPDF(TicketPDF ticketPDF, AbonnementPDF abonnementPDF)
        {
            //Create a byte array that will eventually hold our final PDF
            Byte[] bytes;

            //Boilerplate iTextSharp setup here
            //Create a stream that we can write to, in this case a MemoryStream
            using (var ms = new MemoryStream())
            {

                //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                using (var doc = new Document())
                {

                    //Create a writer that's bound to our PDF abstraction and our stream
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {

                        //Open the document for writing
                        doc.Open();

                        string html = "";
                        string css = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Content/voucher/vouchernew.css"));

                        if (ticketPDF != null)
                        {
                            html = String.Format(System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Content/voucher/vouchernew.html")), ticketPDF.TicketId, ticketPDF.BestellingId, ticketPDF.Prijs, ticketPDF.ThuisploegNaam, ticketPDF.TegenstandersNaam, ticketPDF.StadionNaam, ticketPDF.WedstrijdDatumEnTijd, ticketPDF.StadionAdres, ticketPDF.BezoekerVoornaam, ticketPDF.BezoekerNaam, ticketPDF.BezoekerRijksregisternummer, ticketPDF.BezoekerEmail, DateTime.Now.ToString());

                        }
                        if (abonnementPDF != null)
                        {
                            html = String.Format(System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Content/voucher/abonnement.html")), abonnementPDF.AbonnementId, abonnementPDF.BestellingId, abonnementPDF.Prijs, abonnementPDF.ClubNaam, abonnementPDF.StadionNaam, abonnementPDF.SeizoenJaar, abonnementPDF.BezoekerVoornaam, abonnementPDF.BezoekerNaam, abonnementPDF.BezoekerRijksregisternummer, abonnementPDF.BezoekerEmail, DateTime.Now.ToString());
                        }
                        /**************************************************
                         * Use the XMLWorker to parse HTML and CSS        *
                         * ************************************************/

                        //In order to read CSS as a string we need to switch to a different constructor
                        //that takes Streams instead of TextReaders.
                        //Below we convert the strings into UTF8 byte array and wrap those in MemoryStreams
                        using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(css)))
                        {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)))
                            {
                                //Parse the HTML
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                            }
                        }
                        doc.Close();
                    }
                }

                //After all of the PDF "stuff" above is done and closed but **before** we
                //close the MemoryStream, grab all of the active bytes from the stream
                bytes = ms.ToArray();
            }
            return bytes;
        }



        private Attachment GetAttachment(TicketPDF ticketPDF, AbonnementPDF abonnementPDF)
        {
            var file = new MemoryStream(ConvertHtmlToPDF(ticketPDF, abonnementPDF));
            file.Seek(0, SeekOrigin.Begin);
            Attachment attachment = new Attachment(file, "voucher.pdf", "application/pdf");
            ContentDisposition disposition = attachment.ContentDisposition;
            disposition.CreationDate = System.DateTime.Now;
            disposition.ModificationDate = System.DateTime.Now;
            disposition.DispositionType = DispositionTypeNames.Attachment;
            return attachment;
        }


        [HttpPost]
        public ActionResult Index(BezoekerKoppelen bezoekerKoppelenIn)
        {
            if (ModelState.IsValid)
            {
                bezoekerService = new BezoekerService();


                // WERKWIJZE MET LIST
                ticketService = new TicketService();
                IEnumerable<Ticket> tickets = ticketService.GetNietGekoppeldeTicketsList(User.Identity.GetUserId());


                //// ViewModel maken en opvullen
                BezoekerKoppelen bezoekerKoppelen = new BezoekerKoppelen()
                {
                    NietGekoppeldeTicketsList = tickets.ToList()
                };

                // leg de koppeling
                Debug.WriteLine("valid");
            }
            return View(bezoekerKoppelenIn);
        }
    }
}