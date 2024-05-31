using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class ScholarshipException : BaseCustomException
    {
        public ScholarshipException(string message, HttpStatusCode statusCode) : base(message, "SCH", statusCode) { }

        public class NotFound : ScholarshipException
        {
            public NotFound(Guid id) : base($"Scholarship with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class ItemNotFound : ScholarshipException
        {
            public ItemNotFound(Guid id) : base($"Scholarship type with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class DuplicateBudgets : ScholarshipException
        {
            public DuplicateBudgets() : base($"Given reserve budget list has duplicate budget name, please check the data.", HttpStatusCode.Conflict) { }
        }

        public class BudgetInvalidException : ScholarshipException
        {
            public BudgetInvalidException(string name) : base($"Scholarship reserve budget with given name ({name}) amount is invalid.", HttpStatusCode.Forbidden) { }
        }

        public class FeeItemInvalidException : ScholarshipException
        {
            public FeeItemInvalidException(Guid feeItemId) : base($"Scholarship fee item with given fee item id ({feeItemId}) percentage or amount is invalid.", HttpStatusCode.Forbidden) { }
        }

        public class DuplicateFeeItems : ScholarshipException
        {
            public DuplicateFeeItems() : base($"Given fee item list has duplicate fee item, please check the data.", HttpStatusCode.Conflict) { }
        }
    }
}