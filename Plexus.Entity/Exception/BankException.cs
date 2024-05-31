using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class BankException : BaseCustomException
    {
        public BankException(string message, HttpStatusCode statusCode) : base(message, "BANK", statusCode) { }

        public class NotFound : BankException
        {
            public NotFound(Guid id) : base($"Bank with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class Duplicate : BankException
        {
            public Duplicate(string code = null, string name = null) : base(string.IsNullOrEmpty(code) ? $"Bank with given name ({name}) is already exists."
                                                                                                       : $"Bank with given code ({code}) is already exists.",
                                                                             HttpStatusCode.Conflict)
            { }
        }
    }
}

