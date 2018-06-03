using NUnit.Framework;
using VoetbalTicketStore.Exceptions;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Models.Constants;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.Tests.Service
{
    [TestFixture]
    public class BezoekerServiceTests
    {
        [Test]
        public void AddBezoekerIndienNodigTestParametersNull()
        {
            // arrange
            BezoekerService bezoekerService = new BezoekerService();

            // act + assert
            Assert.Throws<BestelException>(() => bezoekerService.AddBezoekerIndienNodig(null, null, null, null), Constants.ParameterNull);
        }


        [Test]
        public void AddBezoekerIndienNodigBezoekerBestaat()
        {
            BezoekerService bezoekerService = new BezoekerService();

            try
            {
                bezoekerService.AddBezoekerIndienNodig("19092101115", "Bat", "Man", "batcave@alfred.com");

                bezoekerService.AddBezoekerIndienNodig("19092101115", "Bat", "Man", "thejoker@alfred.com");

                Bezoeker bezoeker = bezoekerService.FindBezoeker("19092101115");
                Assert.IsNotNull(bezoeker);
                Assert.AreEqual(bezoeker.Rijksregisternummer, "19092101115");
                Assert.AreEqual(bezoeker.Naam, "Bat");
                Assert.AreEqual(bezoeker.Voornaam, "Man");
                Assert.AreEqual(bezoeker.Email, "thejoker@alfred.com");
            }
            finally
            {
                bezoekerService.RemoveBezoeker("19092101115");
            }


        }

        [Test]
        public void AddBezoekerIndienNodigBezoekerBestaatNiet()
        {
            // arrange
            BezoekerService bezoekerService = new BezoekerService();
            try
            {
                // act
                bezoekerService.AddBezoekerIndienNodig("19092101115", "Bat", "Man", "batcave@alfred.com");

                // assert
                Bezoeker bezoeker = bezoekerService.FindBezoeker("19092101115");

                Assert.IsNotNull(bezoeker);
                Assert.AreEqual(bezoeker.Rijksregisternummer, "19092101115");
                Assert.AreEqual(bezoeker.Naam, "Bat");
                Assert.AreEqual(bezoeker.Voornaam, "Man");
                Assert.AreEqual(bezoeker.Email, "batcave@alfred.com");
            }
            finally
            {
                // cleanup
                bezoekerService.RemoveBezoeker("19092101115");
            }
        }
    }
}