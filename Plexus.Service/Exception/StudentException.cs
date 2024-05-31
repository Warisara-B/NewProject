using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Service.Exception
{
    public class StudentException : BaseCustomException
    {
        public StudentException(string message, HttpStatusCode statusCode) : base(message, "STD", statusCode) { }

        public class NotFound : StudentException
        {
            public NotFound() : base($"Not found student with given id", HttpStatusCode.NotFound) { }
        }

        public class CurrentTermNotFound : StudentException
        {
            public CurrentTermNotFound() : base($"Can't find student current term, please contact admin", HttpStatusCode.NotFound) { }
        }
    }
}

