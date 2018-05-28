using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace VoetbalTicketStore.Service
{
    public class PDFService
    {

        class TicketPDF
        {
        }

        class AbonnementPDF
        {

        }

        Attachment GetAttachment(TicketPDF ticketPDF, AbonnementPDF abonnementPDF)
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
                            html = String.Format(System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Content/voucher/vouchernew.html")), ticketPDF.TicketId, ticketPDF.BestellingId, String.Format("{0:0.00}", ticketPDF.Prijs), ticketPDF.ThuisploegNaam, ticketPDF.TegenstandersNaam, ticketPDF.StadionNaam, ticketPDF.WedstrijdDatumEnTijd, ticketPDF.StadionAdres, ticketPDF.BezoekerVoornaam, ticketPDF.BezoekerNaam, ticketPDF.BezoekerRijksregisternummer, ticketPDF.BezoekerEmail, DateTime.Now.ToString());

                        }
                        if (abonnementPDF != null)
                        {
                            html = String.Format(System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Content/voucher/abonnement.html")), abonnementPDF.AbonnementId, abonnementPDF.BestellingId, String.Format("{0:0.00}", abonnementPDF.Prijs), abonnementPDF.ClubNaam, abonnementPDF.StadionNaam, abonnementPDF.SeizoenJaar, abonnementPDF.BezoekerVoornaam, abonnementPDF.BezoekerNaam, abonnementPDF.BezoekerRijksregisternummer, abonnementPDF.BezoekerEmail, DateTime.Now.ToString());
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
    }
}
