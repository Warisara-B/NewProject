using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
	public class ConditionException : BaseCustomException
	{
        public ConditionException(string message, HttpStatusCode statusCode) : base(message, "CON", statusCode) { }

        public class NotFound : ConditionException
        {
            public NotFound() : base($"Some of given condition isn't found in system.", HttpStatusCode.NotFound) { }

            public NotFound(Guid id) : base($"Condition with given id (id : {id}) isn't found in system.", HttpStatusCode.NotFound) { }
        }
    }
}

