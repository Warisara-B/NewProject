using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class StudentFeeTypeException : BaseCustomException
    {
        public StudentFeeTypeException(string message, HttpStatusCode statusCode) : base(message, "SFT", statusCode) { }

        public class NotFound : StudentFeeTypeException
        {
            public NotFound(Guid id) : base($"Student fee type with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}