using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class InstructorRoleException : BaseCustomException
    {
        public InstructorRoleException(string message, HttpStatusCode statusCode) : base(message, "IR", statusCode) { }

        public class NotFound : InstructorRoleException
        {
            public NotFound(Guid id) : base($"Instructor role with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}