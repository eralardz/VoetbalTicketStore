using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoetbalTicketStore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;
using NUnit.Framework;

// Tests moeten lopen op een testdatabank met officieel aangeleverde dummy data!
namespace VoetbalTicketStore.Service.Tests
{
    [TestClass()]
    public class ClubServiceTests
    {

        private ClubService clubService;


        public ClubServiceTests()
        {
            clubService = new ClubService();
        }

        [TestMethod()]
        public void AllTest()
        {
            IEnumerable<Club> clubs = clubService.All();

            NUnit.Framework.Assert.That(clubs, Has.Count.EqualTo(6));
        }
    }
}