using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoetbalTicketStore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service.Tests
{
    [TestClass()]
    public class TicketServiceTests
    {
        private TicketService ticketService;
        private VakService vakService;

        [TestMethod()]
        public void TicketServiceTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAantalVerkochteTicketsVoorVakTest()
        {



            // arrange
            Vak v = new Vak();
            Wedstrijd w = new Wedstrijd();




            // Twee tickets verkopen
            ticketService.GetAantalVerkochteTicketsVoorVak(v, w);



            // act

            // assert
        }

        [TestMethod()]
        public void AddTicketTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAantalGekochteTicketsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetNietGekoppeldeTicketsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddTicketsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AnnuleerTicketTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetNietGekoppeldeTicketsListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void KoppelBezoekerAanTicketTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindTicketTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAantalTicketsVoorAndereWedstrijdOpDezelfdeDatumTest()
        {
            Assert.Fail();
        }
    }
}