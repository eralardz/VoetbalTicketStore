using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service.Tests
{
    public interface IBezoekerDAO
    {
        Bezoeker FindBezoeker(string rijksregisternummer);
        void AddBezoeker(Bezoeker bezoeker);
        void Wijzigbezoeker(Bezoeker bezoeker);
    }
}