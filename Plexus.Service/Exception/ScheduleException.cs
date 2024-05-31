using Plexus.Utility.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service.Exception
{
    public class ScheduleException : BaseCustomException
    {
        public ScheduleException(string message, HttpStatusCode statusCode) : base(message, "T", statusCode) { }

        public class NotFound : ScheduleException
        {
            public NotFound() : base($"Not found schedule", HttpStatusCode.NotFound) { }
        }

        public class InvalidDateFormat : ScheduleException
        {
            public InvalidDateFormat() : base($"Invalid date format", HttpStatusCode.BadRequest) { }
        }
    }
}
