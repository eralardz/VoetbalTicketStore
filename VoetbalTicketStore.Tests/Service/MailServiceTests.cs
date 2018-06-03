using VoetbalTicketStore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using VoetbalTicketStore.Exceptions;
using VoetbalTicketStore.Models.Constants;
using System.Web.Services.Description;
using System.Net.Mail;
using netDumbster.smtp;
using FakeItEasy;
using System.Net;

namespace VoetbalTicketStore.Tests.Service
{
    [TestFixture]
    public class MailServiceTests
    {

        private SimpleSmtpServer smtpServer;

        [Test]
        public void GenerateMailTestParametersNull()
        {
            // arrange
            MailService mailService = new MailService();

            // act + assert
            Assert.Throws<BestelException>(() => mailService.GenerateMail(null, null), Constants.ParameterNull);
        }

        [Test]
        public void GenerateMailTest()
        {
            // arrange
            MailService mailService = new MailService();
            string email = "TestyTest0r@gmail.com";
            string voornaam = "Fred";

            // act
            MailMessage message = mailService.GenerateMail(email, voornaam);

            // assert message
            Assert.IsNotNull(message);
            Assert.IsTrue(message.To.ToString().Equals(email));
            Assert.IsTrue(message.Body.Contains(voornaam));
        }


        [Test]
        public async Task SendMailAsyncTest()
        {
            SmtpClient client = new SmtpClient("localhost", 25);
            try
            {
                // arrange - SMTP server lokaal hosten via netDumbster, zo kunnen we de email onderscheppen
                smtpServer = SimpleSmtpServer.Start(25);

                // act
                MailService mailService = new MailService(client);

                string email = "TestyTest0r@gmail.com";
                string voornaam = "Fred";
                MailMessage message = mailService.GenerateMail(email, voornaam);

                await mailService.SendMailAsync(message);

                // assert ontvangen mail in test-smtp-server
                var result = smtpServer.ReceivedEmail.First();
                Assert.AreEqual(1, smtpServer.ReceivedEmail.Count());

                SmtpMessage resultEmail = smtpServer.ReceivedEmail.First();
                Assert.AreEqual("vbtstore2018@gmail.com", resultEmail.FromAddress.ToString());
                Assert.IsTrue(resultEmail.ToAddresses.First().ToString().Contains(email));
                Assert.IsTrue(resultEmail.Data.ToString().Contains(voornaam));
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}