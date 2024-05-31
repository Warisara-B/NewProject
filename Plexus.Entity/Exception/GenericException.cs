using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
	public class GenericException : BaseCustomException
	{
        public GenericException(string message, HttpStatusCode statusCode) : base(message, "GE", statusCode) { }

        public class BadRequest : GenericException
        {
            public BadRequest(string parameter) : base($"Invalid given request parameter ({parameter}).", HttpStatusCode.BadRequest) { }
        }
    }
}

