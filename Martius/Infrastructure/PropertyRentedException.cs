using System;

namespace Martius.Infrastructure
{
    public class PropertyRentedException : Exception
    {
        public PropertyRentedException()
        {
        }

        public PropertyRentedException(string message) : base(message)
        {
        }


        public PropertyRentedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}