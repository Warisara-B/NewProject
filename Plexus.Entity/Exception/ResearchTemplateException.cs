using System.Net;
using Plexus.Database.Enum;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class ResearchTemplateException : BaseCustomException
    {
        public ResearchTemplateException(string message, HttpStatusCode statusCode) : base(message, "ResTem", statusCode) { }

        public class NotFound : ResearchTemplateException
        {
            public NotFound(Guid id) : base($"Research template with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
            public NotFound(Guid id, LanguageCode language) : base($"Research template with given id ({id}) does not contains the language {language}.", HttpStatusCode.NotFound) { }
        }
    }
}