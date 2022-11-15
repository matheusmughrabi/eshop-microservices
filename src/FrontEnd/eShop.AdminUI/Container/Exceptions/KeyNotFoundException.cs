using System.Runtime.Serialization;

namespace eShop.AdminUI.Container.Exceptions
{
    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException()
        {
        }

        public KeyNotFoundException(string? message) : base(message)
        {
        }

        public KeyNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected KeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
