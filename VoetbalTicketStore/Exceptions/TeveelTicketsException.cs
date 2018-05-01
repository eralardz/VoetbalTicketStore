using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoetbalTicketStore.Exceptions
{
    public class TeveelTicketsException : Exception
    {
        public TeveelTicketsException(string message) : base(message)
        {
        }

        public TeveelTicketsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}