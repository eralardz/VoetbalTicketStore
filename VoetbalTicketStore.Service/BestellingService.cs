using System;
using System.Collections;
using System.Collections.Generic;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Exceptions;
using VoetbalTicketStore.Models;
using VoetbalTicketStore.Models.Constants;
using System.Linq;

namespace VoetbalTicketStore.Service
{
    public class BestellingService
    {
        private BestellingDAO bestellingDAO;
        private TicketService ticketService;
        private AbonnementService abonnementService;
        private ShoppingCartDataService shoppingCartDataService;

        public BestellingService()
        {
            bestellingDAO = new BestellingDAO();
        }

        public Bestelling CreateNieuweBestellingIndienNodig(string user)
        {
            Bestelling gevondenBestelling = bestellingDAO.FindOpenstaandeBestelling(user);

            if (gevondenBestelling != null)
            {
                // openstaande bestelling teruggeven
                return gevondenBestelling;

            }
            else
            {
                // nieuwe bestelling aanmaken
                Bestelling bestelling = new Bestelling()
                {
                    Bevestigd = false,
                    TotaalPrijs = 0,
                    AspNetUsersId = user,
                    BestelDatum = DateTime.Now
                };

                return bestellingDAO.AddBestelling(bestelling);
            }
        }

        private bool BestellingMagGeplaatstWorden(Bestelling bestelling, string user)
        {
            bool go = true;

            ticketService = new TicketService();
            abonnementService = new AbonnementService();

            // eerste datum in bestelling
            DateTime date = bestelling.ShoppingCartDatas.First().Wedstrijd.DatumEnTijd;


            // Een persoon kan geen tickets kopen voor twee verschillende wedstrijden op dezelfde wedstrijddag
            // Tickets nog niet gekocht
            IEnumerable<IGrouping<string, ShoppingCartData>> group = bestelling.ShoppingCartDatas.GroupBy(t => t.Wedstrijd.DatumEnTijd.ToShortDateString());
            int count = group.First().Count();
            if(count > 1)
            {
                throw new BestelException("Je wil tickets voor verschillende wedstrijden op dezelfde dag kopen. Dit is niet toegestaan!");
            }

            foreach (ShoppingCartData shoppingCartData in bestelling.ShoppingCartDatas)
            {
                if (shoppingCartData.ShoppingCartDataTypeId == 1)
                {
                    // Een persoon kan geen tickets kopen voor twee verschillende wedstrijden op dezelfde wedstrijddag
                    // Tickets reeds gekocht
                    if (ticketService.GetAantalTicketsVoorAndereWedstrijdOpDezelfdeDatum(user, (int)shoppingCartData.WedstrijdId, shoppingCartData.Wedstrijd.DatumEnTijd) > 0)
                    {
                        throw new BestelException("Je hebt al tickets gekocht voor een andere wedstrijd op deze dag, dit is niet toegestaan!");
                    }

                    // maximaal aantal tickets per persoon per wedstrijd
                    if (shoppingCartData.Hoeveelheid + ticketService.GetAantalGekochteTickets(user, (int)shoppingCartData.WedstrijdId) > Constants.MaximaalAantalTicketsPerGebruikerPerWedstrijd)
                    {
                        throw new BestelException("Er mogen maximaal " + Constants.MaximaalAantalTicketsPerGebruikerPerWedstrijd + " tickets per wedstrijd aangekocht worden!");
                    }

                    // is er plaats vrij? Enkel entries met tickets - TODO: enum
                    int aantalVerkochteTickets = ticketService.GetAantalVerkochteTicketsVoorVak(shoppingCartData.Vak, shoppingCartData.Wedstrijd) + abonnementService.GetAantalAbonnementenPerVakVoorThuisPloeg(shoppingCartData.Club, shoppingCartData.Vak);

                    int totaalAantal = aantalVerkochteTickets + shoppingCartData.Hoeveelheid;

                    int rest = shoppingCartData.Vak.MaximumAantalZitplaatsen - totaalAantal;

                    if (rest < 0)
                    {
                        int aantalVrijePlaatsen = shoppingCartData.Vak.MaximumAantalZitplaatsen - aantalVerkochteTickets;
                        throw new BestelException("Er zijn nog slechts " + aantalVrijePlaatsen + " tickets beschikbaar voor " + shoppingCartData.Wedstrijd.Club.Naam + " - " + shoppingCartData.Wedstrijd.Club1.Naam + " op " + shoppingCartData.Wedstrijd.DatumEnTijd + ". Verminder het aantal tickets en probeer opnieuw. Wees snel!");
                    }
                }

            }
            return go;
        }


        public Bestelling FindOpenstaandeBestellingDoorUser(string user)
        {
            return bestellingDAO.FindOpenstaandeBestellingDoorUser(user);
        }

        public decimal BerekenTotaalPrijs(ICollection<ShoppingCartData> shoppingCartDatas)
        {
            decimal totaalPrijs = 0;

            foreach (ShoppingCartData shoppingCartData in shoppingCartDatas)
            {
                totaalPrijs += (shoppingCartData.Prijs * shoppingCartData.Hoeveelheid);
            }

            return totaalPrijs;
        }

        public void RemoveBestelling(string user)
        {
            bestellingDAO.RemoveBestelling(user);
        }

        public void BevestigBestelling(int id)
        {
            Bestelling bestelling = new Bestelling { Id = id, Bevestigd = true };
            bestellingDAO.BevestigBestelling(bestelling);
        }

        public void PlaatsBestelling(Bestelling bestelling, string user)
        {
            // services
            ticketService = new TicketService();
            abonnementService = new AbonnementService();
            shoppingCartDataService = new ShoppingCartDataService();

            IList<Ticket> tickets = new List<Ticket>();
            IList<Abonnement> abonnementen = new List<Abonnement>();

            if (BestellingMagGeplaatstWorden(bestelling, user))
            {
                // maak objecten
                foreach (ShoppingCartData shoppingCartData in bestelling.ShoppingCartDatas)
                {
                    for (int i = 0; i < shoppingCartData.Hoeveelheid; i++)
                    {
                        if (shoppingCartData.ShoppingCartDataTypeId == 1)
                        {

                            Ticket ticket = new Ticket();
                            ticket.Gebruikerid = user;
                            ticket.Prijs = shoppingCartData.Prijs;
                            ticket.Vakid = shoppingCartData.VakId;

                            // nullable attributes
                            if (shoppingCartData.WedstrijdId != null)
                            {
                                ticket.Wedstrijdid = (int)shoppingCartData.WedstrijdId;

                            }
                            ticket.BestellingId = shoppingCartData.BestellingId;

                            tickets.Add(ticket);
                        }
                        else if (shoppingCartData.ShoppingCartDataTypeId == 2)
                        {
                            Abonnement abonnement = new Abonnement()
                            {
                                Clubid = shoppingCartData.Thuisploeg,
                                Prijs = shoppingCartData.Prijs,
                                VakTypeId = shoppingCartData.VakId
                            };

                            abonnementen.Add(abonnement);
                        }
                    }
                }
            }

            // add in bulk
            ticketService.AddTickets(tickets);
            abonnementService.AddAbonnementen(abonnementen);

            // bestelling bevestigen
            BevestigBestelling(bestelling.Id);

            // delete shoppingcartdata
            shoppingCartDataService.RemoveShoppingCartDataVanBestelling(bestelling.Id);
        }
    }
}
