using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoetbalTicketStore.Models.Constants
{
    public static class Constants
    {
        // The recommended naming and capitalization convention is to use Pascal casing for constants.
        public const int MaximaalAantalTicketsPerGebruikerPerWedstrijd = 4;
        public const int TicketVerkoopOpentAantalDagenVoorDeWedstrijd = 30;

        public const bool SeizoenIsBegonnen = false;

        // Exception strings
        public const string ParameterNull = "Er ging iets mis! Probeer het later opnieuw.";
        public const string VroegerDanEenMaand = "Je kan slechts een ticket kopen 1 maand voor de wedstrijd!";
        public const string RijksregisternummerKomtAlvoor = "Dit rijksregisternummer komt al voor in ons systeem!";
        public const string RijksregisternummerOngeldig = "Ongeldig gevormd rijksregisternummer!";


    }
}
