using System;
using System.Runtime.Serialization;

namespace DataAccess.Exceptions
{
    [Serializable]
    public class InvalidUserPasswordException : Exception
    {
        private int id;

        public InvalidUserPasswordException()
        {
        }

        public InvalidUserPasswordException(int id)
        {
            this.id = id;
        }

        public InvalidUserPasswordException(string message) : base(message)
        {
        }

        public InvalidUserPasswordException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidUserPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}