using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Service.Exception
{
    public class AdvisingException : BaseCustomException
    {
        public AdvisingException(string message, HttpStatusCode statusCode) : base(message, "AVS", statusCode) { }

        public class AdvisorNotAssign : AdvisingException
        {
            public AdvisorNotAssign() : base($"Advisor not assign. please contact admin.", HttpStatusCode.Conflict) { }
        }

        public class NotAllowBookAdvisingSlot : AdvisingException
        {
            public NotAllowBookAdvisingSlot() : base($"Not allow request book selected slot.", HttpStatusCode.Conflict) { }
        }

        public class OtherBookSlot : AdvisingException
        {
            public OtherBookSlot() : base($"Selected slot was already booked by other.", HttpStatusCode.Conflict) { }
        }

        public class SlotNotFound : AdvisingException
        {
            public SlotNotFound() : base($"Selected slot not found in system", HttpStatusCode.NotFound) { }
        }

        public class NotAllowUpdateAdvisingSlot : AdvisingException
        {
            public NotAllowUpdateAdvisingSlot() : base($"Not allow update advising slot.", HttpStatusCode.Conflict) { }
        }
    }
}

