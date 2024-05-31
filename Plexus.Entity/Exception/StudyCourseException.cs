using System;
using System.Net;
using Plexus.Database.Enum.Academic;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class StudyCourseException : BaseCustomException
    {
        public StudyCourseException(string message, HttpStatusCode statusCode) : base(message, "STC", statusCode) { }

        public class NotFound : StudyCourseException
        {
            public NotFound(Guid studyCourseId) : base($"Study course with given id ({studyCourseId}) was not found", HttpStatusCode.NotFound) { }

            public NotFound(Guid sectionId, Guid studentId) : base($"Study course with given id (sectionId : {sectionId}, studentId : {studentId}) was not found", HttpStatusCode.NotFound) { }
        }

        public class NotAllowUpdateStatusBackward : StudyCourseException
        {
            public NotAllowUpdateStatusBackward(StudyCourseStatus baseStatus, StudyCourseStatus updateStatus) : base($"Not allow update status from {baseStatus} to {updateStatus}", HttpStatusCode.Conflict) { }
        }
    }
}

