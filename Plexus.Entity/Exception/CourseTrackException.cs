using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class CourseTrackException : BaseCustomException
    {
        public CourseTrackException(string message, HttpStatusCode statusCode) : base(message, "CT", statusCode) { }

        public class NotFound : CourseTrackException
        {
            public NotFound(Guid id) : base($"Course track with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class Duplicate : CourseTrackException
        {
            public Duplicate(string code) : base($"Course track with given code is already exists. (code : {code})", HttpStatusCode.Conflict) { }
        }

        public class DuplicateCourses : CourseTrackException
        {
            public DuplicateCourses() : base($"Given course list has duplicate course, please check the data.", HttpStatusCode.Conflict) { }
        }
    }
}