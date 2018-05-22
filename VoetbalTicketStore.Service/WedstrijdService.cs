using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class WedstrijdService
    {
        private WedstrijdDAO wedstrijdDAO;

        public WedstrijdService()
        {
            wedstrijdDAO = new WedstrijdDAO();
        }

        public IEnumerable<Wedstrijd> All()
        {
            return wedstrijdDAO.All();
        }


        public IEnumerable<Wedstrijd> GetWedstrijdKalenderVanPloeg(Club club)
        {
            return wedstrijdDAO.getWedstrijdKalenderVanPloeg(club);
        }

        public IEnumerable<Wedstrijd> GetUpcomingWedstrijden()
        {
            return wedstrijdDAO.GetUpcomingWedstrijden();
        }

        public Wedstrijd GetWedstrijdById(int id)
        {
            return wedstrijdDAO.getWedstrijdById(id);
        }
    }
}