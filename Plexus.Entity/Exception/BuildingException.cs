using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
	public class BuildingException : BaseCustomException
    {
        public BuildingException(string message, HttpStatusCode statusCode) : base(message, "BUD", statusCode) { }

        public class NotFound : BuildingException
        {
            public NotFound(Guid buildingId) : base($"Building with given id (id : {buildingId}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class Duplicate : BuildingException
        {
            public Duplicate(string code) : base($"Building with given code (code : {code}) is already exists.", HttpStatusCode.Conflict) { }
        }

        public class InvalidAvailableTime : BuildingException
        {
            public InvalidAvailableTime(TimeOnly fromTime, TimeOnly toTime) : base($"Given time ({fromTime} - {toTime}) is invalid.", HttpStatusCode.Conflict) { }
        }
    }
}

