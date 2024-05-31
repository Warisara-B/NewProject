using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class TeachingTypeException : BaseCustomException
    {
        public TeachingTypeException(string message, HttpStatusCode statusCode) : base(message, "TT", statusCode) { }

        public class NotFound : TeachingTypeException
        {
            public NotFound(Guid id) : base($"Teaching type with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}