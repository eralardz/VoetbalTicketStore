using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public interface IBezoekerService
    {
        Bezoeker AddBezoekerIndienNodig(string rijksregisternummer, string naam, string voornaam, string email);

        Bezoeker FindBezoeker(string rijksregisternummer);
    }
}
