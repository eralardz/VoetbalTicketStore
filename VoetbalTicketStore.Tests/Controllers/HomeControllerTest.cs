using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FakeItEasy;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.ViewModel;


//Controller logic should be minimal and not be focused on business logic or infrastructure concerns(for example, data access). Test controller logic, not the framework.Test how the controller behaves based on valid or invalid inputs.Test controller responses based on the result of the business operation it performs.


namespace VoetbalTicketStore.Controllers.Tests
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public async Task IndexTestAsync()
        {
            // arrange
            // hier gebruik van Moq ipv FakeItEasy om de usermanager zelf te mocken (statische methode FindById niet mogelijk in FakeItEasy)
            string username = "test@test.com";
            var identity = new GenericIdentity(username, "");
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, username);
            identity.AddClaim(nameIdentifierClaim);

            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(identity);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);

            var context = new Mock<HttpContextBase>();
            var principal = mockPrincipal.Object;
            context.Setup(x => x.User).Returns(principal);

            var userManagerMock = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);
            userManagerMock.As<IUserPasswordStore<ApplicationUser>>()
                 .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                 .ReturnsAsync(new ApplicationUser() { Id = "id" });


            // FakeItEasy om de ControllerContext te mocken -> gebruik van User.Identity.GetUserId()
            var identity2 = new GenericIdentity("TestUsername");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "f42bf7d5-9a65-4466-b665-e10576b2939d"));

            var fakePrincipal = A.Fake<IPrincipal>();
            A.CallTo(() => fakePrincipal.Identity).Returns(identity2);

            // FakeItEasy -> fake wedstrijdservice
            var fakeWedstrijdService = A.Fake<IWedstrijdService>();
            A.CallTo(() => fakeWedstrijdService.GetAanTeRadenWedstrijdenVoorClub(1, 3)).Returns(null);

            var homeController = new HomeController(fakeWedstrijdService, new UserManager<ApplicationUser>(userManagerMock.Object))
            {
                ControllerContext = A.Fake<ControllerContext>()
            };

            A.CallTo(() => homeController.ControllerContext.HttpContext.User).Returns(fakePrincipal);

            // fake session
            A.CallTo(() => homeController.ControllerContext.HttpContext.Session["AanTeRadenWedstrijden"]).Returns(new List<Wedstrijd>());

            // act
            var result = await homeController.Index();

            // assert result
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);

            // assert viewmodel
            ViewResult resultView = (ViewResult)result;
            Assert.IsInstanceOf(typeof(HomeVM), resultView.Model);
        }

        [Test]
        public void AboutTest()
        {
            // arrange
            HomeController hc = new HomeController();

            // act
            var actResult = hc.About() as ViewResult;

            // assert
            // If you want to test that an action returns the default view, you have to test that the returned view name is empty
            Assert.That(actResult.ViewName, Is.EqualTo(""));
        }

        [Test]
        public void ContactTest()
        {
            // arrange
            HomeController hc = new HomeController();

            // act
            var actResult = hc.Contact() as ViewResult;

            // assert
            // Lege viewname, wordt impliciet ingevuld
            Assert.That(actResult.ViewName, Is.EqualTo(""));
        }
    }
}

