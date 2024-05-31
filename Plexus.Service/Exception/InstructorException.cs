using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Service.Exception
{
    public class InstructorException : BaseCustomException
	{
        public InstructorException(string message, HttpStatusCode statusCode) : base(message, "INS", statusCode) { }

        public class NotFound : InstructorException
        {
            public NotFound() : base($"Not found instructor with given id", HttpStatusCode.NotFound) { }
        }
    }
}

