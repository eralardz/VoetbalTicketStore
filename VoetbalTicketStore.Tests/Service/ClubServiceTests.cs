using VoetbalTicketStore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;
using NUnit.Framework;

// Tests moeten lopen op een testdatabank met de aangeleverde dummy data!
namespace VoetbalTicketStore.Tests.Service
{
    [TestFixture]
    public class ClubServiceTests
    {

        private ClubService clubService;


        public ClubServiceTests()
        {
            clubService = new ClubService();
        }

        [Test]
        public void AllTest()
        {
            // arrange + act
            IEnumerable<Club> clubs = clubService.All();

            // assert
            Assert.That(clubs, Has.Count.EqualTo(6));
        }

        [Test]
        public void GetClubTest()
        {
            // arrange
            int id = 1; // Club Brugge

            // act
            Club club = clubService.GetClub(id);

            // assert
            Assert.That(club.Id == 1);
        }
    }
}