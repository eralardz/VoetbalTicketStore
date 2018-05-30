using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Exceptions;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Models.Constants;

namespace VoetbalTicketStore.Service
{
    public class MailService : IMailService
    {

        private SmtpClient smtpClient;
        public MailService()
        {

        }

        public MailService(SmtpClient smtpClient)
        {
            this.smtpClient = smtpClient;
        }

        public MailMessage GenerateMail(string email, string voornaam)
        {
            if (email == null || voornaam == null)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            // gegevens mail
            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress("vbtstore2018@gmail.com");
            message.Subject = "Uw bestelling bij VoetbalTicketStore";
            var body = "<h1>Beste {0}, </h1><p>Bedankt voor uw bestelling!<p><p>In bijlage vindt u uw voucher terug, vergeet ze niet mee te nemen naar het stadion!</p><p>Met vriendelijke groeten, team Voetbal Ticket Store</p>";
            message.Body = string.Format(body, voornaam);
            message.IsBodyHtml = true;

            return message;
        }

        public async Task SendMailAsync(MailMessage message)
        {
            if (message == null)
            {
                throw new BestelException(Constants.ParameterNull);
            }

            if (smtpClient == null)
            {
                smtpClient = new SmtpClient();
            }

            using (smtpClient)
            {
                await smtpClient.SendMailAsync(message);
            }
        }
    }
}
