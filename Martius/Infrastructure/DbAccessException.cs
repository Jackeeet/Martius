using System;

namespace Martius.Infrastructure
{
    public class DbAccessException : Exception
    {
        public DbAccessException()
        {
        }

        public DbAccessException(string message) : base(message)
        {
        }


        public DbAccessException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}