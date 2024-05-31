using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class FacultyException : BaseCustomException
    {
        public FacultyException(string message, HttpStatusCode statusCode) : base(message, "FAC", statusCode) { }

        public class NotFound : FacultyException
        {
            public NotFound(Guid id) : base($"Faculty with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class Duplicate : FacultyException
        {
            public Duplicate(string code) : base($"Faculty with given code is already exists. (code : {code})", HttpStatusCode.Conflict)
            { }
        }

        public class ContainsDepartments : FacultyException
        {
            public ContainsDepartments(Guid id) : base($"Faculty with given id ({id}) contains one or more departments.", HttpStatusCode.Conflict) { }
        }
    }
}
