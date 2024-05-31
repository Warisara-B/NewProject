using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Service.Exception
{
    public class AuthorizationException : BaseCustomException
	{
        public AuthorizationException(string message, HttpStatusCode statusCode) : base(message, "AUTH", statusCode) { }

        public class InvalidUsernameOrPassword : AuthorizationException
        {
            public InvalidUsernameOrPassword() : base($"Invalid username or password", HttpStatusCode.Forbidden) { }
        }

        public class Unauthorized : AuthorizationException
        {
            public Unauthorized() : base($"Not allow user login, please contact admin", HttpStatusCode.Unauthorized) { }
        }

        public class UsernameDuplicate : AuthorizationException
        {
            public UsernameDuplicate() : base($"Given username is already used by another user", HttpStatusCode.Conflict) { }
        }
    }
}

