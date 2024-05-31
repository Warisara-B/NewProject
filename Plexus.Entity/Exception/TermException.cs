using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class TermException : BaseCustomException
    {
        public TermException(string message, HttpStatusCode statusCode) : base(message, "T", statusCode) { }

        public class NotFound : TermException
        {
            public NotFound(Guid termId) : base($"Term with given id ({termId}) was not found", HttpStatusCode.NotFound) { }
        }

        public class Duplicate : TermException
        {
            public Duplicate() : base($"Given term condition is already exists in system", HttpStatusCode.Conflict) { }
        }

        public class InvalidDateRange : TermException
        {
            public InvalidDateRange() : base($"The specified date range is invalid. The start date must be before the end date.", HttpStatusCode.Conflict) { }
        }
    }
}

