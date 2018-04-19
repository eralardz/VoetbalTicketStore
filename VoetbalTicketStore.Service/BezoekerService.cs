using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Service
{
    public class BezoekerService
    {
        private BezoekerDAO bezoekerDAO;
        public Bezoeker FindBezoeker()
        {
            bezoekerDAO = new BezoekerDAO();
            return null;
        }


        public void AddBezoeker(Bezoeker bezoeker)
        {
            bezoekerDAO = new BezoekerDAO();
            bezoekerDAO.AddBezoeker(bezoeker);
        }

        public Bezoeker FindBezoeker(string rijksregisternummer)
        {
            bezoekerDAO = new BezoekerDAO();
            return bezoekerDAO.FindBezoeker(rijksregisternummer);
        }
    }
}
