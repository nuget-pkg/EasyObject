using System;
using StellarSdk.Model;

namespace StellarSdk.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestError ErrorDetails
        {
            get;
            private set;
        }

        public BadRequestException(BadRequestError err)
        {
            ErrorDetails = err;
        }

        public BadRequestException(string message, BadRequestError err)
        : base(message)
        {
            ErrorDetails = err;
        }
    }
}
