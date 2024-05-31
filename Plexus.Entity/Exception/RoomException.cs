using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class RoomException : BaseCustomException
    {
        public RoomException(string message, HttpStatusCode statusCode) : base(message, "RM", statusCode) { }

        public class NotFound : RoomException
        {
            public NotFound(Guid id) : base($"Room with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class Duplicate : RoomException
        {
            public Duplicate(string code) : base($"Room with given code is already exists. (code : {code})", HttpStatusCode.Conflict)
            { }
        }

        public class DuplicateFacility : RoomException
        {
            public DuplicateFacility() : base("Given facility list has duplicate facility, please check the data.", HttpStatusCode.Conflict) { }
        }
    }
}

