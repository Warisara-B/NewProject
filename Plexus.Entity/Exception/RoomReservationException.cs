using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class RoomReservationException : BaseCustomException
    {
        public RoomReservationException(string message, HttpStatusCode statusCode) : base(message, "RR", statusCode) { }

        public class NotFound : RoomReservationException
        {
            public NotFound(Guid id) : base($"Room reserve request with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class SlotNotFound : RoomReservationException
        {
            public SlotNotFound(Guid id) : base($"Room reserve slot with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class ReserveSlotOverlapped : RoomReservationException
        {
            public ReserveSlotOverlapped() : base($"Given reservation date range is overlapped with another reservation in same date", HttpStatusCode.Conflict) { }
        }

        public class GivenToDateInvalid : RoomReservationException
        {
            public GivenToDateInvalid() : base($"Given to date can't be before from date", HttpStatusCode.Conflict) { }
        }

        public class GivenTimeInvalid : RoomReservationException
        {
            public GivenTimeInvalid() : base($"From time can't be greater than to time", HttpStatusCode.Conflict) { }
        }
    }
}

