using System;
using System.Net;
using Plexus.Database.Enum.Registration;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
	public class PeriodException : BaseCustomException
	{
        public PeriodException(string message, HttpStatusCode statusCode) : base(message, "PR", statusCode) { }

        public class NotFound : PeriodException
        {
            public NotFound(Guid periodId) : base($"Period with given id ({periodId}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class TimeRangeOverlap : PeriodException
        {
            public TimeRangeOverlap(PeriodType type) : base($"Given {type} period time range is overlapped with another period in same term", HttpStatusCode.Conflict) { }
        }
    }
}

