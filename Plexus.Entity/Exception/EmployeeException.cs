using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class EmployeeException : BaseCustomException
    {
        public EmployeeException(string message, HttpStatusCode statusCode) : base(message, "EMP", statusCode) { }

        public class InstructorNotFound : EmployeeException
        {
            public InstructorNotFound(Guid? instructorId) : base($"Instructor with given id ({instructorId}) was not found", HttpStatusCode.NotFound) { }
        }

        public class NotFound : EmployeeException
        {
            public NotFound(Guid employeeId) : base($"Employee with given id ({employeeId} was not found.)", HttpStatusCode.NotFound) { }
        }
    }
}