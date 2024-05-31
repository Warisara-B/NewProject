using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class FeeGroupException : BaseCustomException
    {
        public FeeGroupException(string message, HttpStatusCode statusCode) : base(message, "FG", statusCode) { }

        public class NotFound : FeeGroupException
        {
            public NotFound(Guid id) : base($"Fee group with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}