using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Client.Exceptions
{
    public class StudyPlanException : BaseCustomException
    {

        public StudyPlanException(string message, HttpStatusCode statusCode) : base(message, "SP", statusCode) { }

        public class NotFound : StudyPlanException
        {
            public NotFound(Guid id) : base($"Study plan with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class NoCoursesFound : StudyPlanException
        {
            public NoCoursesFound() : base($"Study plan contains no courses.", HttpStatusCode.NotFound) { }
        }

        public class SemesterExisted : StudyPlanException
        {
            public SemesterExisted() : base($"Semester already existed in the study plan.", HttpStatusCode.Conflict) { }
        }

        public class CourseExisted : StudyPlanException
        {
            public CourseExisted() : base($"Course already existed in the study plan.", HttpStatusCode.Conflict) { }
        }
    }
}

