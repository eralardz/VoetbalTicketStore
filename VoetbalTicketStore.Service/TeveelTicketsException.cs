using System;
using System.Runtime.Serialization;

namespace VoetbalTicketStore.Service
{
    [Serializable]
    public class TeveelTicketsException : Exception
    {
        public TeveelTicketsException()
        {
        }

        public TeveelTicketsException(string message) : base(message)
        {
        }

        public TeveelTicketsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TeveelTicketsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}