using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace VoetbalTicketStore.Service
{
    public interface IMailService
    {
        MailMessage GenerateMail(string email, string voornaam);

        Task SendMailAsync(MailMessage message);
    }
}
