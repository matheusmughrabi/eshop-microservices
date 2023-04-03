using System.Runtime.Serialization;

namespace eShop.ProductApi.Domain.Exceptions
{
    public class InvalidPropertyValueException : Exception
    {
        public InvalidPropertyValueException()
        {
        }

        public InvalidPropertyValueException(string? message) : base(message)
        {
        }

        public InvalidPropertyValueException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidPropertyValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
