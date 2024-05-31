using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
	public class SlotConditionException : BaseCustomException
	{
        public SlotConditionException(string message, HttpStatusCode statusCode) : base(message, "SLOTCON", statusCode) { }

        public class NotFound : SlotConditionException
        {
            public NotFound(Guid id) : base($"Slot condition with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class InvalidConditions : AudienceGroupException
        {
            public InvalidConditions() : base("Slot condition with given conditions is not valid.", HttpStatusCode.Forbidden) { }
        }
    }
}