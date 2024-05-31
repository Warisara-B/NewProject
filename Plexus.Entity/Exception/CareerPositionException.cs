using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class CareerPositionException : BaseCustomException
    {
        public CareerPositionException(string message, HttpStatusCode statusCode) : base(message, "CP", statusCode) { }

        public class NotFound : CareerPositionException
        {
            public NotFound(Guid id) : base($"Career position with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}