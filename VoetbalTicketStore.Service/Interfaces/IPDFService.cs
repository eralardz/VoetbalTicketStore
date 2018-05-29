using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace VoetbalTicketStore.Service
{
    public interface IPDFService
    {
        Attachment GetAttachment();
        void setPDFInfo(bool ticket, int id, int bestellingId, decimal prijs, string thuisploegNaam, string tegenstandersNaam, string adres, string stadionNaam, DateTime datumEnTijd, string bezoekerVoornaam, string bezoekerNaam, string bezoekerRijksregisternummer, string bezoekerEmail);
        Byte[] ConvertHtmlToPDF();
    }
}
