using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Exceptions;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Models.Constants;

namespace VoetbalTicketStore.Service
{
    public class BezoekerService : IBezoekerService
    {
        private BezoekerDAO bezoekerDAO;

        public BezoekerService()
        {
            bezoekerDAO = new BezoekerDAO();
        }

        public BezoekerService(BezoekerDAO bezoekerDAO)
        {
            this.bezoekerDAO = bezoekerDAO;
        }

        public Bezoeker AddBezoekerIndienNodig(string rijksregisternummer, string naam, string voornaam, string email)
        {
            if (rijksregisternummer == null || naam == null || voornaam == null || email == null)
            {
                throw new BestelException(Constants.ParameterNull);
            }
            // zoek bezoeker
            Bezoeker bezoeker = FindBezoeker(rijksregisternummer);
            if (bezoeker == null)
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
            else
            {
                // Nodig om bezoeker te wijzigen? Voornaam, naam en/of emailadres kunnen wijzigen.
                if (!bezoeker.Naam.Equals(naam) || !bezoeker.Voornaam.Equals(voornaam) || !bezoeker.Email.Equals(email))
                {
                    bezoekerDAO.Wijzigbezoeker(new Bezoeker() { Rijksregisternummer = rijksregisternummer, Naam = naam, Voornaam = voornaam, Email = email });
                }
            }
            return bezoeker;
        }

        public Bezoeker FindBezoeker(string rijksregisternummer)
        {
            if (rijksregisternummer != null)
            {
                return bezoekerDAO.FindBezoeker(rijksregisternummer);
            }
            else
            {
                throw new BestelException(Constants.ParameterNull);
            }
        }

        public void RemoveBezoeker(string rijksregisternummer)
        {
            if (rijksregisternummer != null)
            {
                bezoekerDAO.RemoveBezoeker(rijksregisternummer);

            } else
            {
                throw new BestelException(Constants.ParameterNull);
            }
        }
    }
}
