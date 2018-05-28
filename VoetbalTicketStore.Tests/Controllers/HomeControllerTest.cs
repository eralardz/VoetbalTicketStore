using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using VoetbalTicketStore;
using VoetbalTicketStore.Controllers;


//Controller logic should be minimal and not be focused on business logic or infrastructure concerns(for example, data access). Test controller logic, not the framework.Test how the controller behaves based on valid or invalid inputs.Test controller responses based on the result of the business operation it performs.


namespace VoetbalTicketStore.Controllers.Tests
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void IndexTest()
        {
            //arrange
            var obj = new HomeController();

            //act
            ViewResult vr = obj.Index();

            //assert
            // Lege viewname, wordt impliciet ingevuld
            Assert.That(vr.ViewName, Is.EqualTo("Index"));
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

