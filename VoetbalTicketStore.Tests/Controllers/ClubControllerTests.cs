using VoetbalTicketStore.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.ViewModel;
using NUnit.Framework;
using System.Web.Mvc;

namespace VoetbalTicketStore.Controllers.Tests
{
    [TestFixture]
    public class ClubControllerTests
    {
        [Test]
        public void IndexTest()
        {
            ClubController clubController = new ClubController();

            // act 
            var result = clubController.Index();

            // assert result
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);

            // assert viewmodel
            ViewResult resultView = (ViewResult)result;
            Assert.IsInstanceOf(typeof(ClubOverview), resultView.Model);
        }
    }
}