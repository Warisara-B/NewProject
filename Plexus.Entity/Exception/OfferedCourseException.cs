using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class OfferedCourseException : BaseCustomException
    {
        public OfferedCourseException(string message, HttpStatusCode statusCode) : base(message, "OC", statusCode) { }

        public class NotEnoughSeats : OfferedCourseException
        {
            public NotEnoughSeats(int seatLimit) : base($"Reserved seat exceeds seat limit ({seatLimit})", HttpStatusCode.Forbidden) { }
        }

        public class JointNotEnoughSeats : OfferedCourseException
        {
            public JointNotEnoughSeats(int seatLimit) : base($"Joint section's seat exceeds seat limit ({seatLimit})", HttpStatusCode.Forbidden) { }
        }

        public class JointDuplicate : OfferedCourseException
        {
            public JointDuplicate() : base("Given joint section list has duplicate course and number, please check the data.", HttpStatusCode.Conflict) { }
        }
    }
}