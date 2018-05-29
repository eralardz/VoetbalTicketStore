using System;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Web.Mvc;
using System.Security.Principal;
using System.Security.Claims;
using FakeItEasy;
using VoetbalTicketStore.ViewModel;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.Models;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

namespace VoetbalTicketStore.Controllers.Tests
{
    [TestFixture]
    public class BezoekerControllerTests
    {
        [Test]
        public void IndexTest()
        {
            //arrange - mock voor ASP.Net Identity via FakeItEasy
            var identity = new GenericIdentity("TestUsername");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "f42bf7d5-9a65-4466-b665-e10576b2939d"));

            var fakePrincipal = A.Fake<IPrincipal>();
            A.CallTo(() => fakePrincipal.Identity).Returns(identity);

            var bezoekerController = new BezoekerController
            {
                ControllerContext = A.Fake<ControllerContext>()
            };

            A.CallTo(() => bezoekerController.ControllerContext.HttpContext.User).Returns(fakePrincipal);

            // act 
            var result = bezoekerController.Index();

            // assert result
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);

            // assert viewmodel
            ViewResult resultView = (ViewResult)result;
            Assert.IsInstanceOf(typeof(BezoekerKoppelen), resultView.Model);

            // assert message - bron is geen redirect dus null
            Assert.IsNull(resultView.ViewBag.Msg);
        }

        [Test]
        public void KoppelTest()
        {
            // TODO mock user manager... needs more research
        }

        [Test]
        public void KoppelTestViewModelNull()
        {
            // arrange
            BezoekerController bezoekerController = new BezoekerController();

            // act
            var result = bezoekerController.Koppel(null);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }

        [Test]
        public async Task KoppelPostAsyncTestViewModelNull()
        {
            // arrange
            BezoekerController bezoekerController = new BezoekerController();

            // act
            var result = await bezoekerController.KoppelPostAsync(null);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }

        [Test]
        public async Task KoppelPostAsyncTestInvalidModelState()
        {
            BezoekerController bezoekerController = new BezoekerController();
            bezoekerController.ModelState.AddModelError("test", "test");

            var result = await bezoekerController.KoppelPostAsync(new BezoekerKoppelen());


            // assert result
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);

            // assert viewmodel
            ViewResult resultView = (ViewResult)result;
            Assert.IsInstanceOf(typeof(BezoekerKoppelen), resultView.Model);
        }


        [Test]
        public async Task KoppelPostAsyncTest()
        {
            // arrange
            var fakeBezoekerService = A.Fake<IBezoekerService>();
            A.CallTo(() => fakeBezoekerService.AddBezoekerIndienNodig("90050207940", ("De Wispelaere"), "Laurens", "laurens.dewispelaere@gmail.com")).Returns(new Bezoeker());

            var fakeTicketService = A.Fake<ITicketService>();
            A.CallTo(() => fakeTicketService.KoppelBezoekerAanTicket(0, "90050207940")).DoesNothing();
            A.CallTo(() => fakeTicketService.FindTicket(0)).Returns(new Ticket()
            {
                Id = 1337,
                Gebruikerid = new Guid().ToString(),
                Prijs = new decimal(13.37),
                BestellingId = 1337,
                Wedstrijd = new Wedstrijd()
                {
                    Club = new Club()
                    {
                        Naam = "FakeTeamThuis"
                    },
                    Club1 = new Club()
                    {
                        Naam = "FakeTeamBezoek"
                    },
                    Stadion = new Stadion()
                    {
                        Naam = "FakeStadion",
                        Adres = "FakeAdres"
                    },
                    DatumEnTijd = DateTime.Now
                },
                Bezoeker = new Bezoeker()
                {
                    Voornaam = "Laurens",
                    Naam = "De Wiseplaere",
                    Rijksregisternummer = "90050207940",
                    Email = "laurens.dewispelaere@gmail.com"
                }
            });

            var fakePDFService = A.Fake<IPDFService>();
            A.CallTo(() => fakePDFService.setPDFInfo(true, 1337, 1337, new decimal(13.37), "FakeTeamThuis", "FakeTeamBezoek", "FakeAdres", "FakeStadion", DateTime.Now, "Laurens", "De Wispelaere", "90050207940", "laurens.dewispelaere@gmail.com")).DoesNothing();
            A.CallTo(() => fakePDFService.GetAttachment()).Returns(new Attachment(Path.Combine(TestContext.CurrentContext.TestDirectory, @"Content\attachment.txt"), MediaTypeNames.Text.Plain));

            var fakeBestellingService = A.Fake<IBestellingService>();

            var fakeMailService = A.Fake<IMailService>();
            A.CallTo(() => fakeMailService.SendMailAsync(new System.Net.Mail.MailMessage())).Returns(null);

            var fakeAbonnementService = A.Fake<IAbonnementService>();
            BezoekerController bezoekerController = new BezoekerController(fakeBezoekerService, fakeBestellingService, fakeTicketService, fakeAbonnementService, fakePDFService, fakeMailService);

            // act
            var result = await bezoekerController.KoppelPostAsync(new BezoekerKoppelen() { TeWijzigenBezoeker = new Bezoeker() { Rijksregisternummer = "90050207940", Naam = "De Wispelaere", Voornaam = "Laurens", Email = "laurens.dewispelaere@gmail.com" } });

            // assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsTrue(bezoekerController.TempData["msg"].ToString().StartsWith("Uw ticket werd bevestigd."));

            var resultCast = (RedirectToRouteResult)result;
            Assert.AreEqual("Index", resultCast.RouteValues["action"]);
        }


        [Test]
        public void GenerateTicketPDFTest()
        {
            // arrange
            var fakePDFService = A.Fake<IPDFService>();
            A.CallTo(() => fakePDFService.setPDFInfo(true, 1337, 1337, new decimal(13.37), "FakeTeamThuis", "FakeTeamBezoek", "FakeAdres", "FakeStadion", DateTime.Now, "Laurens", "De Wispelaere", "90050207940", "laurens.dewispelaere@gmail.com")).DoesNothing();

            BezoekerController bezoekerController = new BezoekerController(new BezoekerService(), new BestellingService(), new TicketService(), new AbonnementService(), fakePDFService, new MailService());

            // act
            var result = bezoekerController.GenerateTicketPDF(new BestellingVM());

            // assert
            Assert.IsInstanceOf(typeof(FileContentResult), result);

            var resultCast = (FileContentResult)result;
            Assert.AreEqual(resultCast.ContentType, "application/pdf");
            Assert.AreEqual(resultCast.FileDownloadName, "voucher.pdf");
        }

        [Test]
        public void GenerateTicketPDFTestViewModelNull()
        {
            // arrange
            BezoekerController bezoekerController = new BezoekerController();

            // act
            var result = bezoekerController.GenerateTicketPDF(null);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }

        [Test]
        public void GenerateAbonnementPDFTest()
        {
            // arrange
            var fakePDFService = A.Fake<IPDFService>();
            A.CallTo(() => fakePDFService.setPDFInfo(true, 1337, 1337, new decimal(13.37), "FakeTeamThuis", "FakeTeamBezoek", "FakeAdres", "FakeStadion", DateTime.Now, "Laurens", "De Wispelaere", "90050207940", "laurens.dewispelaere@gmail.com")).DoesNothing();

            BezoekerController bezoekerController = new BezoekerController(new BezoekerService(), new BestellingService(), new TicketService(), new AbonnementService(), fakePDFService, new MailService());

            // act
            var result = bezoekerController.GenerateAbonnementPDF(new BestellingVM());

            // assert
            Assert.IsInstanceOf(typeof(FileContentResult), result);

            var resultCast = (FileContentResult)result;
            Assert.AreEqual(resultCast.ContentType, "application/pdf");
            Assert.AreEqual(resultCast.FileDownloadName, "abonnement.pdf");
        }

        [Test]
        public void GenerateAbonnementPDFTestViewModelNull()
        {
            // arrange
            BezoekerController bezoekerController = new BezoekerController();

            // act
            var result = bezoekerController.GenerateAbonnementPDF(null);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }
    }
}