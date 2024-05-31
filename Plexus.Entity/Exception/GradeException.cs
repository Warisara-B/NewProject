using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
	public class GradeException : BaseCustomException
	{
        public GradeException(string message, HttpStatusCode statusCode) : base(message, "G", statusCode) { }

        public class NotFound : GradeException
        {
            public NotFound(Guid id) : base($"Grade with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class LetterDuplicate : GradeException
        {
            public LetterDuplicate(string letter) : base($"Grade with given letter ({letter}) is already exists in system.", HttpStatusCode.Conflict) { }
        }
    }
}

