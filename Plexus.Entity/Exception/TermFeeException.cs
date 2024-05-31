using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
	public class TermFeeException : BaseCustomException
	{
		public TermFeeException(string message, HttpStatusCode statusCode) : base(message, "TF", statusCode) { }

        public class PackageNotFound : TermFeeException
        {
            public PackageNotFound(Guid id) : base($"Term fee package with given id ({id}) is not found.", HttpStatusCode.Conflict) { }
        }

        public class ItemNotFound : TermFeeException
        {
            public ItemNotFound(Guid id) : base($"Term fee item with given id ({id}) is not found.", HttpStatusCode.Conflict) { }
        }
    }
}

