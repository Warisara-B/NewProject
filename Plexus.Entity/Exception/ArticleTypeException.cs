using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class ArticleTypeException : BaseCustomException
    {
        public ArticleTypeException(string message, HttpStatusCode statusCode) : base(message, "AS", statusCode) { }

        public class NotFound : ArticleTypeException
        {
            public NotFound(Guid id) : base($"Article type with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}