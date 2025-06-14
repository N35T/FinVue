using System.Runtime.Serialization;

namespace FinVue.Core.Exceptions;
public class UserException : Exception {
    public UserException() {
    }

    public UserException(string? message) : base(message) {
    }

    public UserException(string? message, Exception? innerException) : base(message, innerException) {
    }

    protected UserException(SerializationInfo info, StreamingContext context) : base(info, context) {
    }
}