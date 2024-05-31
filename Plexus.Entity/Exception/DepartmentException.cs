using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class DepartmentException : BaseCustomException
    {
        public DepartmentException(string message, HttpStatusCode statusCode) : base(message, "DEP", statusCode) { }

        public class NotFound : DepartmentException
        {
            public NotFound(Guid id) : base($"Department with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class Duplicate : DepartmentException
        {
            public Duplicate(string code) : base($"Department with given code is already exists. (code : {code})", HttpStatusCode.Conflict)
            { }
        }
    }
}
