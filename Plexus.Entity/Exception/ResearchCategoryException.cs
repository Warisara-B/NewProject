using System.Net;
using Plexus.Database.Model.Research;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class ResearchCategoryException : BaseCustomException
    {
        public ResearchCategoryException(string message, HttpStatusCode statusCode) : base(message, "RC", statusCode) { }

        public class NotFound : ResearchCategoryException
        {
            public NotFound(Guid id) : base($"Research category with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}

