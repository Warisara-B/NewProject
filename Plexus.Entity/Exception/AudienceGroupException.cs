using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class AudienceGroupException : BaseCustomException
    {
        public AudienceGroupException(string message, HttpStatusCode statusCode) : base(message, "AG", statusCode) { }

        public class NotFound : AudienceGroupException
        {
            public NotFound(Guid id) : base($"Audience group with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class InvalidConditions : AudienceGroupException
        {
            public InvalidConditions() : base("Audience group with given conditions is not valid.", HttpStatusCode.Forbidden) { }
        }
    }
}