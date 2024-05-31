using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class CurriculumException : BaseCustomException
    {
        public CurriculumException(string message, HttpStatusCode statusCode) : base(message, "CRCL", statusCode) { }

        public class NotFound : CurriculumException
        {
            public NotFound(Guid curriculumId) : base($"Curriculum with given id ({curriculumId}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class VersionNotFound : CurriculumException
        {
            public VersionNotFound(Guid curriculumVersionId) : base($"Curriculum version with given id ({curriculumVersionId}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class CourseGroupNotFound : CurriculumException
        {
            public CourseGroupNotFound(Guid curriculumCourseGroupId) : base($"Curriculum course group with given id ({curriculumCourseGroupId}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class FreeElectiveSubGroupException : CurriculumException
        {
            public FreeElectiveSubGroupException() : base($"Free elective group type can't have parent or sub coursegroup", HttpStatusCode.Conflict) { }
        }

        public class FreeElectiveCourseException : CurriculumException
        {
            public FreeElectiveCourseException() : base($"Not allow set course for free elective group type", HttpStatusCode.Conflict) { }
        }

        public class NonElectiveIgnoreCourseException : CurriculumException
        {
            public NonElectiveIgnoreCourseException() : base($"Not allow set ignore course for non free elective group type", HttpStatusCode.Conflict) { }
        }

        public class NotInCurriculumVersionException : CurriculumException
        {
            public NotInCurriculumVersionException(Guid courseId) : base($"Course with given id ({courseId}) is not in curriculum version", HttpStatusCode.Conflict) { }
        }

        public class ReversedCorequisiteException : CurriculumException
        {
            public ReversedCorequisiteException(Guid courseId, Guid corequisiteCourseId) : base($"There is reverse of course and corequisite course with given id ({courseId}) and ({corequisiteCourseId})", HttpStatusCode.Conflict) { }
        }

        public class InBlackListCourseException : CurriculumException
        {
            public InBlackListCourseException(Guid courseId) : base($"Given course (Id : {courseId}) is currently set in version blacklist.", HttpStatusCode.Conflict) { }
        }

        public class InCurriculumCourseException : CurriculumException
        {
            public InCurriculumCourseException(Guid courseId) : base($"Given course (Id : {courseId}) is currently set in curriculum courses.", HttpStatusCode.Conflict) { }
        }

        public class InCurriculumCoRequisiteException : CurriculumException
        {
            public InCurriculumCoRequisiteException(Guid courseId) : base($"Remove course (Id : {courseId}) is currently set in version corequisite course.", HttpStatusCode.Conflict) { }
        }

        public class InCurriculumCourseEquiException : CurriculumException
        {
            public InCurriculumCourseEquiException(Guid courseId) : base($"Remove course (Id : {courseId}) is currently set in version courses equivalent.", HttpStatusCode.Conflict) { }
        }
    }
}

