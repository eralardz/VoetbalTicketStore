using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoetbalTicketStore.Service
{
    public interface ITicketService
    {
        void AnnuleerTicket(int ticketId);
    }
}
