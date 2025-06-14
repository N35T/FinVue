using System.Runtime.Serialization;

namespace FinVue.Core.Exceptions;
public class CategoryException : Exception {
    public CategoryException() {
    }

    public CategoryException(string? message) : base(message) {
    }

    public CategoryException(string? message, Exception? innerException) : base(message, innerException) {
    }

    protected CategoryException(SerializationInfo info, StreamingContext context) : base(info, context) {
    }
}
