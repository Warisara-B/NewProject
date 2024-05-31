using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class CampusException : BaseCustomException
    {
        public CampusException(string message, HttpStatusCode statusCode) : base(message, "CP", statusCode) { }

        public class NotFound : CampusException
        {
            public NotFound(Guid id) : base($"Campus with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class Duplicate : CampusException
        {
            public Duplicate(string code) : base($"Campus with given code is already exists. (code : {code})", HttpStatusCode.Conflict)
            { }
        }
    }
}

