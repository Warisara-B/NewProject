using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class CourseException : BaseCustomException
    {
        public CourseException(string message, HttpStatusCode statusCode) : base(message, "C", statusCode) { }

        public class NotFound : CourseException
        {
            public NotFound(Guid id) : base($"Course with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}

