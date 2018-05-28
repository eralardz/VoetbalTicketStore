using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class BezoekerService : IBezoekerService
    {

        private BezoekerDAO bezoekerDAO;

        public BezoekerService()
        {
            bezoekerDAO = new BezoekerDAO();
        }

        public Bezoeker AddBezoekerIndienNodig(string rijksregisternummer, string naam, string voornaam, string email)
        {
            // zoek bezoeker
            Bezoeker bezoeker = FindBezoeker(rijksregisternummer);
            if(bezoeker == null)
            {
                bezoeker = new Bezoeker()
                {
                    Rijksregisternummer = rijksregisternummer,
                    Naam = naam,
                    Voornaam = voornaam,
                    Email = email
                };

                bezoekerDAO.AddBezoeker(bezoeker);
            }
            return bezoeker;
        }

        public Bezoeker FindBezoeker(string rijksregisternummer)
        {
            return bezoekerDAO.FindBezoeker(rijksregisternummer);
        }
    }
}
