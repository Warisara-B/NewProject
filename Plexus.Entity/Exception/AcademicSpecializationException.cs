using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class AcademicSpecializationException : BaseCustomException
    {
        public AcademicSpecializationException(string message, HttpStatusCode statusCode) : base(message, "AS", statusCode) { }

        public class NotFound : AcademicSpecializationException
        {
            public NotFound(Guid academicSpecializationId) : base($"Academic specialization with given id ({academicSpecializationId}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}