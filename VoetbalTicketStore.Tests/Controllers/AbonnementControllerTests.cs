using VoetbalTicketStore.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using VoetbalTicketStore.ViewModel;
using System.Web.Mvc;

namespace VoetbalTicketStore.Controllers.Tests
{
    [TestFixture]
    public class AbonnementControllerTests
    {
        // Checking the type of the viewmodel returned is valuable because, if the wrong type of viewmodel is returned, MVC will throw a runtime exception. You can prevent this from happening in production by running a unit test. If the test fails, then the view may throw an exception in production. 
        [Test]
        public void BuyTest()
        {
            // arrange
            AbonnementController abonnementController = new AbonnementController();
            ClubOverview clubOverview = new ClubOverview();

            // act
            var result = abonnementController.Buy(clubOverview);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);

            ViewResult resultView = (ViewResult)result;
            Assert.IsInstanceOf(typeof(AbonnementBuy), resultView.Model);
        }

        [Test]
        public void BuyTestViewModelNull()
        {
            // arrange
            AbonnementController abonnementController = new AbonnementController();

            // act
            var result = abonnementController.Buy(null);

            // assert
            Assert.IsInstanceOf(typeof(HttpStatusCodeResult), result);

            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual(400, httpResult.StatusCode);
        }
    }
}