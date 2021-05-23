using System;

namespace Martius.Infrastructure
{
    public class EntityExistsException : Exception
    {
        public EntityExistsException()
        {
        }

        public EntityExistsException(string message) : base(message)
        {
        }

        public EntityExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}