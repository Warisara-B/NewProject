using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class CourseTopicException : BaseCustomException
    {
        public CourseTopicException(string message, HttpStatusCode statusCode) : base(message, "C", statusCode) { }

        public class NotFound : CourseException
        {
            public NotFound(Guid id) : base($"Course with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}

