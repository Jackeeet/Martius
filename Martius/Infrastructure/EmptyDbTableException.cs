using System;

namespace Martius.Infrastructure
{
    public class EmptyDbTableException : Exception
    {
        public EmptyDbTableException()
        {
        }

        public EmptyDbTableException(string message) : base(message)
        {
        }

        public EmptyDbTableException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}