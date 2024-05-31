using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Service.Exception
{
	public class NewsException : BaseCustomException
    {
        public NewsException(string message, HttpStatusCode statusCode) : base(message, "NWS", statusCode) { }

        public class NotFound : NewsException
        {
            public NotFound() : base($"Not found news with given id", HttpStatusCode.NotFound) { }
        }
    }
}

