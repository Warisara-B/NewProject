using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class PrerequisiteException : BaseCustomException
    {
        public PrerequisiteException(string message, HttpStatusCode statusCode) : base(message, "PREREQ", statusCode) { }

        public class InvalidGradeCondition : PrerequisiteException
        {
            public InvalidGradeCondition() : base("Invalid grade condition.", HttpStatusCode.Forbidden) { }
        }

        public class InvalidGPACondition : PrerequisiteException
        {
            public InvalidGPACondition() : base("Invalid gpa condition.", HttpStatusCode.Forbidden) { }
        }

        public class InvalidCreditCondition : PrerequisiteException
        {
            public InvalidCreditCondition() : base("Invalid credit condition.", HttpStatusCode.Forbidden) { }
        }

        public class InvalidTermCountCondition : PrerequisiteException
        {
            public InvalidTermCountCondition() : base("Invalid term count condition.", HttpStatusCode.Forbidden) { }
        }

        public class InvalidFacultyCondition : PrerequisiteException
        {
            public InvalidFacultyCondition() : base("Invalid faculty condition.", HttpStatusCode.Forbidden) { }
        }

        public class PrerequisiteConditionFail : PrerequisiteException
        {
            public PrerequisiteConditionFail(string courseCode, string courseName) : base($"Given course ({courseCode} {courseName}) one or more prerequisites failed.", HttpStatusCode.BadRequest) { }
        }
    }
}