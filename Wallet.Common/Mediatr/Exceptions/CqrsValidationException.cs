using System.Runtime.Serialization;

namespace Wallet.Common.Mediatr.Exceptions
{
    /// <summary>
    /// Cqrs Validation Exception.
    /// </summary>
    public class CqrsValidationException : Exception
    {
        protected CqrsValidationException(SerializationInfo info, in StreamingContext context) 
            : base(info, context)
        {
        }

        public CqrsValidationException(string? message) 
            : base(message)
        {
        }

        public CqrsValidationException(string? message, Exception? innerException) 
            : base(message, innerException)
        {
        }
    }
}