using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class RoomTypeException : BaseCustomException
    {
        public RoomTypeException(string message, HttpStatusCode statusCode) : base(message, "RMT", statusCode) { }

        public class NotFound : RoomTypeException
        {
            public NotFound(Guid id) : base($"Room type with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class Duplicate : RoomTypeException
        {
            public Duplicate(string code) : base($"Room type with given code is already exists. (code : {code})", HttpStatusCode.Conflict)
            { }
        }
    }
}

