using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
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
    public class BezoekerController : Controller
    {

        private BezoekerService bezoekerService;
        private BestellingService bestellingService;
        private TicketService ticketService;

        // GET: Bezoeker
        public ActionResult Index()
        {
            bezoekerService = new BezoekerService();
            bestellingService = new BestellingService();



            // WERKWIJZE MET LIST
            ticketService = new TicketService();
            IEnumerable<Ticket> tickets = ticketService.GetNietGekoppeldeTicketsList(User.Identity.GetUserId());


            // ViewModel maken en opvullen
            BezoekerKoppelen bezoekerKoppelen = new BezoekerKoppelen()
            {
                NietGekoppeldeTicketsList = tickets.ToList()
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

                // rijksregisternummer aan ticket koppelen
                ticketService = new TicketService();
                ticketService.KoppelBezoekerAanTicket(bezoekerKoppelen.TeWijzigenTicket, bezoeker.Rijksregisternummer);

                // voucher aanmaken en doormailen
                var message = new MailMessage();
                message.To.Add(new MailAddress(bezoekerKoppelen.TeWijzigenBezoeker.Email));
                message.From = new MailAddress("laurens.dewispelaere@gmail.com");
                // replace with valid value
                message.Subject = "Ticket gekocht!";

                // ticket + alle info nodig om voucher te genereren
                Ticket ticket = ticketService.FindTicket(bezoekerKoppelen.TeWijzigenTicket);
                message.Attachments.Add(BezoekerController.GetAttachment(bezoekerKoppelen, ticket));

                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                message.Body = string.Format(body, bezoekerKoppelen.TeWijzigenBezoeker.Naam, bezoekerKoppelen.TeWijzigenBezoeker.Email, "bla");
                message.IsBodyHtml = true;


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

        // This tool parses (X)HTML snippets and the associated CSS and converts them to PDF.
        // XMLWorker is an extra component for iText®. The first XML to PDF implementation, is a new version of the old HTMLWorker that used to be shipped with iText.
        private static MemoryStream ConvertHtmlToPDF(BezoekerKoppelen bezoekerKoppelen, Ticket ticket)
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

                        string html = String.Format(System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Content/voucher/voucher.html")), ticket.Id, ticket.BestellingId, ticket.Wedstrijd.Club.Naam, ticket.Wedstrijd.Club1.Naam, ticket.Wedstrijd.DatumEnTijd.ToShortDateString(), ticket.Wedstrijd.DatumEnTijd.ToShortTimeString(), ticket.Wedstrijd.Stadion.Naam, ticket.Wedstrijd.Stadion.Adres, bezoekerKoppelen.TeWijzigenBezoeker.Voornaam, bezoekerKoppelen.TeWijzigenBezoeker.Naam, bezoekerKoppelen.TeWijzigenBezoeker.Rijksregisternummer, bezoekerKoppelen.TeWijzigenBezoeker.Email, DateTime.Now.ToString());

                        string css = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Content/voucher/voucher.css"));

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
            return new MemoryStream(bytes);
        }


        private static Attachment GetAttachment(BezoekerKoppelen bezoekerKoppelen, Ticket ticket)
        {
            var file = ConvertHtmlToPDF(bezoekerKoppelen, ticket);
            file.Seek(0, SeekOrigin.Begin);
            Attachment attachment = new Attachment(file, "test.pdf", "application/pdf");
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