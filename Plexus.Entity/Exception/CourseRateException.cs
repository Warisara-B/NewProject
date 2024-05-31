using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
	public class CourseRateException : BaseCustomException
	{
        public CourseRateException(string message, HttpStatusCode statusCode) : base(message, "CR", statusCode) { }

        public class NotFound : CourseRateException
        {
            public NotFound(Guid id) : base($"CourseRate with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class DuplicateIndexes : CourseRateException
        {
            public DuplicateIndexes() : base($"Given rate list has duplicate rateType and index, please check the data.", HttpStatusCode.Conflict) { }
        }
    }
}

