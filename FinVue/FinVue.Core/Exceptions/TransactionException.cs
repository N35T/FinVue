﻿using System.Runtime.Serialization;

namespace FinVue.Core.Exceptions;
public class TransactionException : Exception {
    public TransactionException() {
    }

    public TransactionException(string? message) : base(message) {
    }

    public TransactionException(string? message, Exception? innerException) : base(message, innerException) {
    }

    protected TransactionException(SerializationInfo info, StreamingContext context) : base(info, context) {
    }
}
