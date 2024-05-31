using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class AcademicLevelException : BaseCustomException
    {
        public AcademicLevelException(string message, HttpStatusCode statusCode) : base(message, "AL", statusCode) { }

        public class NotFound : AcademicLevelException
        {
            public NotFound(Guid id) : base($"Academic level with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class Duplicate : AcademicLevelException
        {
            public Duplicate(string code) : base($"Academic level with given code is already exists. (code : {code})", HttpStatusCode.Conflict)
            { }
        }
    }
}

