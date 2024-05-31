using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class ExclusionConditionException : BaseCustomException
    {
        public ExclusionConditionException(string message, HttpStatusCode statusCode) : base(message, "EXCC", statusCode) { }

        public class NotFound : ExclusionConditionException
        {
            public NotFound(Guid id) : base($"Exclusion condition with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class ConditionNotSpecify : ExclusionConditionException
        {
            public ConditionNotSpecify() : base("Please specify conditions for exclusion conditions.", HttpStatusCode.Forbidden) { }
        }
    }
}
