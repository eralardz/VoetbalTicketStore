using NUnit.Framework;
using System.Web.Mvc;
using System.Security.Principal;
using System.Security.Claims;
using FakeItEasy;
using VoetbalTicketStore.ViewModel;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.Controllers.Tests
{
    [TestFixture]
    public class BestellingControllerTests
    {
        [Test]
        public void IndexTest()
        {
            //arrange - mock voor ASP.Net Identity via FakeItEasy
            var identity = new GenericIdentity("TestUsername");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "f42bf7d5-9a65-4466-b665-e10576b2939d"));

            var fakePrincipal = A.Fake<IPrincipal>();
            A.CallTo(() => fakePrincipal.Identity).Returns(identity);

            var bestellingController = new BestellingController
            {
                ControllerContext = A.Fake<ControllerContext>()
            };

            A.CallTo(() => bestellingController.ControllerContext.HttpContext.User).Returns(fakePrincipal);

            // act
            var result = bestellingController.Index();

            // assert
            // result
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);

            // viewmodel
            ViewResult resultView = (ViewResult)result;
            Assert.IsInstanceOf(typeof(BestellingVM), resultView.Model);

            // message (geen redirect hier na het kopen)
            Assert.IsNull(resultView.ViewBag.Msg);
        }


        [Test]
        public void AnnulerenTest()
        {
            // arrange
            var fakeTicketService = A.Fake<ITicketService>();
            A.CallTo(() => fakeTicketService.AnnuleerTicket(0)).DoesNothing();
            var fakeBestellingService = A.Fake<BestellingService>();
            BestellingController bestellingController = new BestellingController(fakeTicketService, fakeBestellingService);

            // act
            var result = bestellingController.Annuleren(new BestellingVM() { TicketId = 0 });


            // assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);

            var resultCast = (RedirectToRouteResult)result;
            Assert.AreEqual("Index", resultCast.RouteValues["action"]);
        }

        [Test]
        public void AnnulerenTestViewModelNull()
        {
            // arrange
            BestellingController bestellingController = new BestellingController(null, null);

            // act
            var result = bestellingController.Annuleren(null);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }
    }
}