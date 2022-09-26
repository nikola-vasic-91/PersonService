﻿namespace PersonService.Domain.Exceptions
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException()
        {
        }

        public OperationFailedException(string message)
            : base(message)
        {
        }

        public OperationFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
