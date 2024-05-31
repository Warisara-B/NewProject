using System;
using System.Net;
using Plexus.Database.Enum;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class PlanException : BaseCustomException
    {
        public PlanException(string message, HttpStatusCode statusCode) : base(message, "P", statusCode) { }

        public class PlanWithSameCourseAlreadyExists : PlanException
        {
            public PlanWithSameCourseAlreadyExists(PlanType type, IEnumerable<Guid> courseIds) : base($"User already have {type} plan with same selected courses. (Course Ids : {string.Join(",", courseIds)})", HttpStatusCode.Conflict) { }
        }

        public class PlanWithSameSectionAlreadyExists : PlanException
        {
            public PlanWithSameSectionAlreadyExists(IEnumerable<Guid> sectionIds) : base($"User already have save plan with same selected sections. (Section Ids : {string.Join(",", sectionIds)})", HttpStatusCode.Conflict) { }
        }

        public class NotFound : PlanException
        {
            public NotFound(Guid id) : base($"Plan with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}