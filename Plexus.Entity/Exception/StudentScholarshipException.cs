using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class StudentScholarshipException : BaseCustomException
    {
        public StudentScholarshipException(string message, HttpStatusCode statusCode) : base(message, "SSCH", statusCode) { }

        public class NotFound : StudentScholarshipException
        {
            public NotFound(Guid id) : base($"Student Scholarship with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class BudgetNotFound : StudentScholarshipException
        {
            public BudgetNotFound(Guid id) : base($"Student Scholarship reserved budget with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class Duplicate : StudentScholarshipException
        {
            public Duplicate(string code) : base($"Student with given code is already exists. (code : {code})", HttpStatusCode.Conflict) { }

            public Duplicate(IEnumerable<string> codes) : base($"Student with given codes is already exists. (codes : {string.Join(",", codes)})", HttpStatusCode.Conflict) { }
        }

        public class ReservedBudgetNotFound : StudentScholarshipException
        {
            public ReservedBudgetNotFound(Guid id) : base($"Student Scholarship Reserve Budet with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class ExceedLimitBalance : StudentScholarshipException
        {
            public ExceedLimitBalance() : base($"Given pool balance is greater than total remaining of scholarship balance", HttpStatusCode.Conflict) { }
        }

        public class NotAllowSetBudgetLessThanZero : StudentScholarshipException
        {
            public NotAllowSetBudgetLessThanZero() : base($"Not allow set budget balance less than zero", HttpStatusCode.Conflict) { }
        }

        public class NotAllowUpdateReserveBalance : StudentScholarshipException
        {
            public NotAllowUpdateReserveBalance() : base($"Not allow update reserve balance below already used budget", HttpStatusCode.Conflict) { }
        }

        public class NotAllowAdjustPositionAmount : StudentScholarshipException
        {
            public NotAllowAdjustPositionAmount() : base($"Not allow adjust negative amount for reserved budget", HttpStatusCode.Conflict) { }
        }

        public class NotAllowAdjustOverRemaining : StudentScholarshipException
        {
            public NotAllowAdjustOverRemaining() : base($"Pool remaining balance after adjustment can't be less than zero", HttpStatusCode.Conflict) { }
        }
    }
}

