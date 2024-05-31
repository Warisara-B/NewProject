using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class SectionException : BaseCustomException
    {
        public SectionException(string message, HttpStatusCode statusCode) : base(message, "SEC", statusCode) { }

        public class NotFound : SectionException
        {
            public NotFound(Guid sectionId) : base($"Section with given id ({sectionId}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class Duplicate : SectionException
        {
            public Duplicate(string number) : base($"Section with given number is already exists. (number : {number})", HttpStatusCode.Conflict) { }
        }

        public class NotEnough : SectionException
        {
            public NotEnough(int availableSeat) : base($"Seat is not enough. (available seat : {availableSeat})", HttpStatusCode.Forbidden) { }
        }

        public class NotAllowCreateDefaultSeat : SectionException
        {
            public NotAllowCreateDefaultSeat() : base("Not allow create seat of DEFAULT type.", HttpStatusCode.Forbidden) { }
        }

        public class DetailOverlapped : SectionException
        {
            public DetailOverlapped() : base($"Given section detail information has overlapped period", HttpStatusCode.Conflict) { }
        }

        public class ExamOverlapped : SectionException
        {
            public ExamOverlapped() : base($"Given examination time range is overlapped with another examination in same date", HttpStatusCode.Conflict) { }
        }

        public class SeatNotFound : SectionException
        {
            public SeatNotFound(Guid seatId) : base($"Seat with given id isn't found (id : {seatId})", HttpStatusCode.NotFound) { }
        }

        public class NotAllowDeleteSeat : SectionException
        {
            public NotAllowDeleteSeat() : base($"Not allow delete seat of DEFAULT, RESERVE types and already used", HttpStatusCode.Conflict) { }
        }

        public class NotEnoughRemainingSeat : SectionException
        {
            public NotEnoughRemainingSeat(int remainingSeat) : base($"Section doesn't have enough seat to comply with given request (remaining : {remainingSeat})", HttpStatusCode.Conflict) { }
        }

        public class NotAllowUpdateSeatLessThanAlreadyUsed : SectionException
        {
            public NotAllowUpdateSeatLessThanAlreadyUsed(int usedAmount) : base($"Total seat amount can't be lower than already used seat (use amount : {usedAmount})", HttpStatusCode.Conflict) { }
        }

        public class NotAllowNestedJoint : SectionException
        {
            public NotAllowNestedJoint() : base("Not allow joint section has nested joint section.", HttpStatusCode.Forbidden) { }
        }

        public class NotAllowUpdateJointSection : SectionException
        {
            public NotAllowUpdateJointSection() : base("Not allow to update joint section.", HttpStatusCode.Forbidden) { }
        }

        public class DefaultSeatNotFound : SectionException
        {
            public DefaultSeatNotFound(Guid sectionId) : base($"Default seat with given section id isn't found (id : {sectionId})", HttpStatusCode.NotFound) { }
        }

        public class NotAllowClose : SectionException
        {
            public NotAllowClose(Guid sectionId) : base($"Section with given id ({sectionId} cannot be closed.)", HttpStatusCode.Conflict) { }
        }
    }
}

