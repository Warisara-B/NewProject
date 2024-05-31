using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class PublicationException : BaseCustomException
    {
        public PublicationException(string message, HttpStatusCode statusCode) : base(message, "AS", statusCode) { }

        public class NotFound : PublicationException
        {
            public NotFound(Guid id) : base($"Publication with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}