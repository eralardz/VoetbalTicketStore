using NUnit.Framework;
using VoetbalTicketStore.Service;
using FakeItEasy;
using System.Web.Mvc;
using System.Collections.Generic;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Controllers.Tests
{
    [TestFixture]
    public class WedstrijdControllerTests
    {
        [Test]
        public void IndexTest()
        {
            // arrange
            WedstrijdController wedstrijdController = new WedstrijdController();

            // act
            var result = wedstrijdController.Index();

            // assert result
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);

            // assert viewmodel
            ViewResult resultView = (ViewResult)result;
            Assert.IsInstanceOf(typeof(IEnumerable<Wedstrijd>), resultView.Model);
        }


        [Test]
        public void WedstrijdKalenderTestViewModelNull()
        {
            // arrange
            WedstrijdController wedstrijdController = new WedstrijdController();

            // act
            var result = wedstrijdController.WedstrijdKalender(null);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }


        [Test]
        public void WedstrijdKalenderTest()
        {
            // arrange
            var fakeWedstrijdService = A.Fake<IWedstrijdService>();
            A.CallTo(() => fakeWedstrijdService.GetWedstrijdKalenderVanPloeg(null)).Returns(new List<Wedstrijd>());

            WedstrijdController wedstrijdController = new WedstrijdController(fakeWedstrijdService);

            // act
            var result = wedstrijdController.WedstrijdKalender(new Club());


            // assert result
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);

            // assert viewmodel
            ViewResult resultView = (ViewResult)result;
            Assert.IsInstanceOf(typeof(IEnumerable<Wedstrijd>), resultView.Model);
        }
    }
}