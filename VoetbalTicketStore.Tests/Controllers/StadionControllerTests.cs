using VoetbalTicketStore.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FakeItEasy;
using VoetbalTicketStore.Service.Interfaces;
using System.Web.Mvc;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Controllers.Tests
{
    [TestFixture]
    public class StadionControllerTests
    {
        [Test]
        public void IndexTest()
        {
            // arrange
            var fakeStadionService = A.Fake<IStadionService>();
            A.CallTo(() => fakeStadionService.All()).Returns(new List<Stadion>());
            StadionController stadionController = new StadionController(fakeStadionService);

            // act
            var result = stadionController.Index();

            // assert result
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);

            // assert viewmodel
            ViewResult resultView = (ViewResult)result;
            Assert.IsInstanceOf(typeof(IEnumerable<Stadion>), resultView.Model);
        }
    }
}