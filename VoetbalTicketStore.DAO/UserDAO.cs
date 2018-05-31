using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.DAO.Interfaces;
using VoetbalTicketStore.Models;
using System.Data.Entity;

namespace VoetbalTicketStore.DAO
{
    public class UserDAO : IUserDAO
    {
        public bool RijksregisternummerKomtAlVoor(string rijksregisternummer)
        {
            using (var db = new VoetbalstoreEntities())
            {
                List<AspNetUser> users = db.AspNetUsers.Where(u => u.Rijksregisternummer.Equals(rijksregisternummer)).ToList();
                if (users.Count() > 0)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }
    }
}
