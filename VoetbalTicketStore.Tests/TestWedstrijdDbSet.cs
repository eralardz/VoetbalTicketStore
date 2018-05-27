using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoetbalTicketStore.Models;

namespace VoetbalTicketStore.Tests
{
    class TestWedstrijdDbSet : TestDBSet<Wedstrijd>
    {
        public override Wedstrijd Find(params object[] keyValues)
        {
            return this.SingleOrDefault(wedstrijd => wedstrijd.Id == (int)keyValues.Single());
        }
    }
}
