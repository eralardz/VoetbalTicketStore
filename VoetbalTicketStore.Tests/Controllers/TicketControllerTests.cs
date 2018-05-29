using VoetbalTicketStore.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Web.Mvc;
using FakeItEasy;
using VoetbalTicketStore.Service;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Service.Interfaces;
using VoetbalTicketStore.ViewModel;

namespace VoetbalTicketStore.Controllers.Tests
{
    [TestFixture]
    public class TicketControllerTests
    {

        [Test]
        public void BuyTestInvalidId()
        {
            // arrange
            TicketController ticketController = new TicketController();

            // act
            var result = ticketController.Buy(-1);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }


        [Test]
        public void BuyTest()
        {
            // arrange
            var fakeWedstrijdService = A.Fake<IWedstrijdService>();
            A.CallTo(() => fakeWedstrijdService.GetWedstrijdById(0)).Returns(new Wedstrijd());

            var fakeVakService = A.Fake<IVakService>();
            A.CallTo(() => fakeVakService.GetVakkenInStadion(0)).Returns(new List<Vak>());
            A.CallTo(() => fakeVakService.BerekenPrijzenBijVakken(null, null)).DoesNothing();
            A.CallTo(() => fakeVakService.BerekenAantalVrijePlaatsen(null, null, null)).DoesNothing();

            TicketController ticketController = new TicketController(fakeWedstrijdService, fakeVakService);

            // act
            var result = ticketController.Buy(0);

            // assert result
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);

            // assert viewmodel
            ViewResult resultView = (ViewResult)result;
            Assert.IsInstanceOf(typeof(TicketWedstrijd), resultView.Model);
        }


        [Test]
        public void ConfirmTestInvalidParameters()
        {
            // arrange
            TicketController ticketController = new TicketController();

            // act
            var result = ticketController.Confirm(-1, 0, new decimal(0.0), 0, 0, 0, "");

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }

        [Test]
        public void ConfirmTest()
        {
            // arrange
            TicketController ticketController = new TicketController();

            // act
            var result = ticketController.Confirm(0, 0, new decimal(0.0), 0, 0, 0, "test");

            // assert result
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(PartialViewResult), result);

            // assert viewmodel
            PartialViewResult resultView = (PartialViewResult)result;
            Assert.IsInstanceOf(typeof(TicketConfirm), resultView.Model);
        }
    }
}