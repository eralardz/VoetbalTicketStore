using VoetbalTicketStore.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Web.Mvc;
using System.Security.Principal;
using System.Security.Claims;
using FakeItEasy;
using VoetbalTicketStore.ViewModel;

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
        public void KoppelPostAsyncTest()
        {
            Assert.Fail();
        }

        [Test]
        public void GenerateTicketPDFTest()
        {
            Assert.Fail();
        }

        [Test]
        public void GenerateAbonnementPDFTest()
        {
            Assert.Fail();
        }

        [Test]
        public void IndexTest1()
        {
            Assert.Fail();
        }
    }
}