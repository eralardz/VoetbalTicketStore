using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoetbalTicketStore.Exceptions
{
    public class BestelException : Exception
    {
        public BestelException(string message) : base(message)
        {
        }

        public BestelException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}