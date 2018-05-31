using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO;
using VoetbalTicketStore.Service.Interfaces;

namespace VoetbalTicketStore.Service
{
    public class UserService : IUserService
    {
        private UserDAO userDAO;

        public UserService()
        {
            userDAO = new UserDAO();
        }

        public bool RijksregisternummerKomtAlVoor(string rijksregisternummer)
        {
            return userDAO.RijksregisternummerKomtAlVoor(rijksregisternummer);
        }
    }
}
