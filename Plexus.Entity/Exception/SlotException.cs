using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
	public class SlotException : BaseCustomException
	{
        public SlotException(string message, HttpStatusCode statusCode) : base(message, "SLOT", statusCode) { }

        public class NotFound : SlotException
        {
            public NotFound(Guid id) : base($"Slot with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}

