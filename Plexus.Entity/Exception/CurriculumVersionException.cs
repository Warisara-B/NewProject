using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class CurriculumVersionException : BaseCustomException
    {
        public CurriculumVersionException(string message, HttpStatusCode statusCode) : base(message, "CV", statusCode) { }

        public class NotFound : CurriculumVersionException
        {
            public NotFound(Guid id) : base($"Curriculum version with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class InstructorNotFound : CurriculumVersionException
        {
            public InstructorNotFound(Guid id) : base($"Curriculum instructor with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}