using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class AcademicPositionException : BaseCustomException
    {
        public AcademicPositionException(string message, HttpStatusCode statusCode) : base(message, "AP", statusCode) { }

        public class NotFound : AcademicPositionException
        {
            public NotFound(Guid id) : base($"Academic position with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}

