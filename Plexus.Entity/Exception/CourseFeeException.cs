using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
	public class CourseFeeException : BaseCustomException
	{
        public CourseFeeException(string message, HttpStatusCode statusCode) : base(message, "CF", statusCode) { }

        public class NotFound : CourseFeeException
        {
            public NotFound(Guid id) : base($"CourseFee with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}

