using NUnit.Framework;
using System.Web.Mvc;
using System.Security.Principal;
using System.Security.Claims;
using FakeItEasy;

namespace VoetbalTicketStore.Controllers.Tests
{
    [TestFixture]
    public class BestellingControllerTests
    {
        [Test]
        public void IndexTest()
        {
            // arrange - mock voor ASP.Net Identity via FakeItEasy
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
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void AnnulerenTest()
        {
            Assert.Fail();
        }
    }
}