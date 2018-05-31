using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Security.Claims;
using FakeItEasy;
using System.Web.Mvc;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.Models;
using NUnit.Framework;
using VoetbalTicketStore.ViewModel;
using Moq;
using System.Web;
using Microsoft.AspNet.Identity;
using VoetbalTicketStore.Exceptions;

namespace VoetbalTicketStore.Controllers.Tests
{
    [TestFixture]
    public class ShoppingCartControllerTests
    {
        [Test]
        public void IndexTest()
        {
            // arrange
            var identity = new GenericIdentity("TestUsername");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "f42bf7d5-9a65-4466-b665-e10576b2939d"));

            var fakePrincipal = A.Fake<IPrincipal>();
            A.CallTo(() => fakePrincipal.Identity).Returns(identity);

            var fakeBestellingService = A.Fake<IBestellingService>();
            A.CallTo(() => fakeBestellingService.FindOpenstaandeBestellingDoorUser(new Guid().ToString())).Returns(new Bestelling());
            A.CallTo(() => fakeBestellingService.BerekenTotaalPrijs(new List<ShoppingCartData>())).Returns(new decimal(0.0));

            var shoppingCartController = new ShoppingCartController(fakeBestellingService, new ShoppingCartDataService(), null)
            {
                ControllerContext = A.Fake<ControllerContext>()
            };

            A.CallTo(() => shoppingCartController.ControllerContext.HttpContext.User).Returns(fakePrincipal);

            // act
            var result = shoppingCartController.Index();

            // assert result
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);

            // assert viewmodel
            ViewResult resultView = (ViewResult)result;
            Assert.IsInstanceOf(typeof(ShoppingCart), resultView.Model);

            // assert message - bron is geen redirect dus null
            Assert.IsNull(resultView.ViewBag.Error);
            Assert.IsNull(resultView.ViewBag.Success);
        }

        [Test]
        public void AddTest()
        {
            // arrange
            var identity = new GenericIdentity("TestUsername");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "f42bf7d5-9a65-4466-b665-e10576b2939d"));

            var fakePrincipal = A.Fake<IPrincipal>();
            A.CallTo(() => fakePrincipal.Identity).Returns(identity);

            var fakeBestellingService = A.Fake<IBestellingService>();
            A.CallTo(() => fakeBestellingService.CreateNieuweBestellingIndienNodig(new Guid().ToString())).Returns(new Bestelling());

            var fakeShoppingCartDataService = A.Fake<IShoppingCartDataService>();
            A.CallTo(() => fakeShoppingCartDataService.AddToShoppingCart(0, new decimal(0.0), 0, 0, 0, 0, 0, "")).Returns(null);


            var shoppingCartController = new ShoppingCartController(fakeBestellingService, fakeShoppingCartDataService, null)
            {
                ControllerContext = A.Fake<ControllerContext>()
            };

            A.CallTo(() => shoppingCartController.ControllerContext.HttpContext.User).Returns(fakePrincipal);

            // act
            var result = shoppingCartController.Add(new TicketConfirm());

            // assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsTrue(shoppingCartController.TempData["success"].ToString().Equals("Uw winkelwagentje werd aangepast!"));

            var resultCast = (RedirectToRouteResult)result;
            Assert.AreEqual("Index", resultCast.RouteValues["action"]);
        }


        [Test]
        public void AddTestViewModelNull()
        {
            // arrange
            ShoppingCartController shoppingCartController = new ShoppingCartController();

            // act
            var result = shoppingCartController.Add(null);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }

        [Test]
        public void AddAbonnementTestViewModelNull()
        {
            // arrange
            ShoppingCartController shoppingCartController = new ShoppingCartController();

            // act
            var result = shoppingCartController.AddAbonnement(null);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }


        [Test]
        public void AddAbonnementTest()
        {
            // arrange
            var identity = new GenericIdentity("TestUsername");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "f42bf7d5-9a65-4466-b665-e10576b2939d"));

            var fakePrincipal = A.Fake<IPrincipal>();
            A.CallTo(() => fakePrincipal.Identity).Returns(identity);

            var fakeBestellingService = A.Fake<IBestellingService>();
            A.CallTo(() => fakeBestellingService.CreateNieuweBestellingIndienNodig(new Guid().ToString())).Returns(new Bestelling());

            var fakeShoppingCartDataService = A.Fake<IShoppingCartDataService>();
            A.CallTo(() => fakeShoppingCartDataService.AddAbonnementToShoppingCart(0, new decimal(0.0), 0, 0, 0, "")).Returns(null);


            var shoppingCartController = new ShoppingCartController(fakeBestellingService, fakeShoppingCartDataService, null)
            {
                ControllerContext = A.Fake<ControllerContext>()
            };

            A.CallTo(() => shoppingCartController.ControllerContext.HttpContext.User).Returns(fakePrincipal);

            // act
            var result = shoppingCartController.Add(new TicketConfirm());

            // assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsTrue(shoppingCartController.TempData["success"].ToString().Equals("Uw winkelwagentje werd aangepast!"));

            var resultCast = (RedirectToRouteResult)result;
            Assert.AreEqual("Index", resultCast.RouteValues["action"]);
        }

        [Test]
        public void RemoveTestInvalidId()
        {
            // arrange
            ShoppingCartController shoppingCartController = new ShoppingCartController();

            // act
            var result = shoppingCartController.Remove(-1);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }

        [Test]
        public void RemoveTest()
        {
            // arrange
            var fakeShoppingCartDataService = A.Fake<IShoppingCartDataService>();
            A.CallTo(() => fakeShoppingCartDataService.RemoveShoppingCartData(0)).DoesNothing();
            ShoppingCartController shoppingCartController = new ShoppingCartController(new BestellingService(), fakeShoppingCartDataService, null);

            // act
            var result = shoppingCartController.Remove(0);

            // assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);

            var resultCast = (RedirectToRouteResult)result;
            Assert.AreEqual("Index", resultCast.RouteValues["action"]);
        }

        [Test]
        public void AdjustAmountTestViewModelNull()
        {
            // arrange
            ShoppingCartController shoppingCartController = new ShoppingCartController();

            // act
            var result = shoppingCartController.AdjustAmount(null);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }


        [Test]
        public void AdjustAmountTest()
        {
            // arrange
            var identity = new GenericIdentity("TestUsername");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "f42bf7d5-9a65-4466-b665-e10576b2939d"));

            var fakePrincipal = A.Fake<IPrincipal>();
            A.CallTo(() => fakePrincipal.Identity).Returns(identity);

            var fakeShoppingCartDataService = A.Fake<IShoppingCartDataService>();
            A.CallTo(() => fakeShoppingCartDataService.AdjustAmount(0, 0)).DoesNothing();

            var shoppingCartController = new ShoppingCartController(new BestellingService(), fakeShoppingCartDataService, null)
            {
                ControllerContext = A.Fake<ControllerContext>()
            };

            A.CallTo(() => shoppingCartController.ControllerContext.HttpContext.User).Returns(fakePrincipal);

            // act
            var result = shoppingCartController.AdjustAmount(new ShoppingCart());

            // assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);

            var resultCast = (RedirectToRouteResult)result;
            Assert.AreEqual("Index", resultCast.RouteValues["action"]);
        }

        [Test]
        public void ClearTestViewModelNull()
        {
            // arrange
            ShoppingCartController shoppingCartController = new ShoppingCartController();

            // act
            var result = shoppingCartController.Clear(null);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }

        [Test]
        public void ClearTest()
        {
            // arrange
            var fakeShoppingCartDataService = A.Fake<IShoppingCartDataService>();
            A.CallTo(() => fakeShoppingCartDataService.RemoveShoppingCartDataVanBestelling(0)).DoesNothing();
            ShoppingCartController shoppingCartController = new ShoppingCartController(new BestellingService(), fakeShoppingCartDataService, null);

            // act
            var result = shoppingCartController.Clear(new ShoppingCart());

            // assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);

            var resultCast = (RedirectToRouteResult)result;
            Assert.AreEqual("Index", resultCast.RouteValues["action"]);
        }

        [Test]
        public void FinaliseTestViewModelNull()
        {
            // arrange
            ShoppingCartController shoppingCartController = new ShoppingCartController();

            // act
            var result = shoppingCartController.Finalise(null);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }

        [Test]
        public void FinaliseTest()
        {
            // arrange
            // hier gebruik van Moq ipv FakeItEasy om de usermanager zelf te mocken (statische methode FindById intercepteren niet mogelijk in FakeItEasy)
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

            // FakeItEasy -> fake bestellingservice
            var fakeBestellingService = A.Fake<IBestellingService>();
            A.CallTo(() => fakeBestellingService.FindOpenstaandeBestellingDoorUser("")).Returns(null);
            A.CallTo(() => fakeBestellingService.PlaatsBestelling(new Bestelling(), "")).DoesNothing();
            A.CallTo(() => fakeBestellingService.GetMeestGekochteThuisploeg("")).Returns(0);

            var shoppingCartController = new ShoppingCartController(fakeBestellingService, new ShoppingCartDataService(), new UserManager<ApplicationUser>(userManagerMock.Object))
            {
                ControllerContext = A.Fake<ControllerContext>(),
            };

            A.CallTo(() => shoppingCartController.ControllerContext.HttpContext.User).Returns(fakePrincipal);

            // act
            var result = shoppingCartController.Finalise(new ShoppingCart());

            // assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsTrue(shoppingCartController.TempData["msg"].ToString().Equals("Uw bestelling werd succesvol afgerond!"));

            var resultCast = (RedirectToRouteResult)result;
            Assert.AreEqual("Index", resultCast.RouteValues["action"]);
            Assert.AreEqual("Bezoeker", resultCast.RouteValues["controller"]);
        }

        [Test]
        public void FinaliseTestException()
        {
            // arrange
            // hier gebruik van Moq ipv FakeItEasy om de usermanager zelf te mocken (statische methode FindById intercepteren niet mogelijk in FakeItEasy)
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

            // FakeItEasy -> fake bestellingservice
            var fakeBestellingService = A.Fake<IBestellingService>();
            A.CallTo(() => fakeBestellingService.FindOpenstaandeBestellingDoorUser("")).Returns(null);
            A.CallTo(() => fakeBestellingService.PlaatsBestelling(new Bestelling(), "")).DoesNothing();
            A.CallTo(() => fakeBestellingService.GetMeestGekochteThuisploeg("")).Returns(0);

            var shoppingCartController = new ShoppingCartController(fakeBestellingService, new ShoppingCartDataService(), new UserManager<ApplicationUser>(userManagerMock.Object))
            {
                ControllerContext = A.Fake<ControllerContext>(),
            };

            string exceptionMessage = "MaaktNietUit";

            A.CallTo(() => shoppingCartController.ControllerContext.HttpContext.User).Throws(new BestelException(exceptionMessage));

            // act
            var result = shoppingCartController.Finalise(new ShoppingCart());

            // assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsTrue(shoppingCartController.TempData["error"].ToString().Equals(exceptionMessage));

            var resultCast = (RedirectToRouteResult)result;
            Assert.AreEqual("Index", resultCast.RouteValues["action"]);
        }
    }
}