using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoetbalTicketStore.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.Claims;
using FakeItEasy;
using System.Web.Mvc;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.Controllers.Tests
{
    [TestClass()]
    public class ShoppingCartControllerTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            // arrange
            var identity = new GenericIdentity("TestUsername");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "f42bf7d5-9a65-4466-b665-e10576b2939d"));

            var fakePrincipal = A.Fake<IPrincipal>();
            A.CallTo(() => fakePrincipal.Identity).Returns(identity);

            var bezoekerController = new BezoekerController
            {
                ControllerContext = A.Fake<ControllerContext>()
            };

            A.CallTo(() => bezoekerController.ControllerContext.HttpContext.User).Returns(fakePrincipal);


            var fakeBestellingService = A.Fake<IBestellingService>();


            // act



            // assert
        }

        [TestMethod()]
        public void AddTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddAbonnementTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AdjustAmountTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ClearTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FinaliseTest()
        {
            Assert.Fail();
        }
    }
}