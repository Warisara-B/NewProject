using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class FacilityException : BaseCustomException
    {
        public FacilityException(string message, HttpStatusCode statusCode) : base(message, "FACI", statusCode) { }

        public class NotFound : ExclusionConditionException
        {
            public NotFound(Guid id) : base($"Facility with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}