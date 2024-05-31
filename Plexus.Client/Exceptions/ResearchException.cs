using System.Net;
using Plexus.Entity.Exception;
using Plexus.Utility.Exception;

namespace Plexus.Client.Exceptions
{
    public class ResearchException : BaseCustomException
	{

        public ResearchException(string message, HttpStatusCode statusCode) : base(message, "RE", statusCode) { }

        public class TemplateNotFound : ResearchTemplateException
        {
            public TemplateNotFound(Guid id) : base($"Research template with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }
    }
}

