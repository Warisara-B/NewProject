using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class WithdrawalException : BaseCustomException
    {
        public WithdrawalException(string message, HttpStatusCode statusCode) : base(message, "WR", statusCode) { }

        public class NotFound : WithdrawalException
        {
            public NotFound(Guid id) : base($"Withdrawal request with given id ({id}) is not found.", HttpStatusCode.NotFound) { }
        }

        public class NotAllowRequestWithdrawal : WithdrawalException
        {
            public NotAllowRequestWithdrawal(Guid studyCourseId) : base($"Not allow withdrawal for study course with given id ({studyCourseId}) since it's already have grade.", HttpStatusCode.Conflict) { }
        }

        public class NotAllowReverseStatus : WithdrawalException
        {
            public NotAllowReverseStatus() : base($"Not allow reverse update withdrawal request status", HttpStatusCode.Conflict) { }
        }
    }
}

