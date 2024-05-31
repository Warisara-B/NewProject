using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class RateTypeException : BaseCustomException
    {
        public RateTypeException(string message, HttpStatusCode statusCode) : base(message, "RT", statusCode) { }

        public class NotFound : RateTypeException
        {
            public NotFound(Guid id) : base($"Rate type with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}