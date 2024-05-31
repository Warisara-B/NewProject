using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class EmployeeGroupException : BaseCustomException
    {
        public EmployeeGroupException(string message, HttpStatusCode statusCode) : base(message, "EG", statusCode) { }

        public class NotFound : EmployeeGroupException
        {
            public NotFound(Guid id) : base($"Employee group with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}