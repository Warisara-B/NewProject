using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Service.Exception
{
    public class TermException : BaseCustomException
    {
        public TermException(string message, HttpStatusCode statusCode) : base(message, "T", statusCode) { }

        public class NotFound : TermException
        {
            public NotFound() : base($"Not found term with given id", HttpStatusCode.NotFound) { }
        }
    }
}