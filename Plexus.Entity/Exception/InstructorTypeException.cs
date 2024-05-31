using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class InstructorTypeException : BaseCustomException
    {
        public InstructorTypeException(string message, HttpStatusCode statusCode) : base(message, "IT", statusCode) { }

        public class NotFound : InstructorTypeException
        {
            public NotFound(Guid id) : base($"Instructor type with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}