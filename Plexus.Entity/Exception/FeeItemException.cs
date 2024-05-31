using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
	public class FeeItemException : BaseCustomException
	{
        public FeeItemException(string message, HttpStatusCode statusCode) : base(message, "FI", statusCode) { }

        public class NotFound : FeeItemException
        {
            public NotFound(Guid feeItemId) : base($"Fee item with given id ({feeItemId}) was not found", HttpStatusCode.NotFound) { }
        }
    }
}

