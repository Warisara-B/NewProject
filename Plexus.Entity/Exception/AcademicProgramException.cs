using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class AcademicProgramException : BaseCustomException
    {
        public AcademicProgramException(string message, HttpStatusCode statusCode) : base(message, "AP", statusCode) { }

        public class NotFound : AcademicProgramException
        {
            public NotFound(Guid id) : base($"Academic program with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}