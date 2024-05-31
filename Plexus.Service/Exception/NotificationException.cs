using Plexus.Utility.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service.Exception
{
    public class NotificationException : BaseCustomException
    {
        public NotificationException(string message, HttpStatusCode statusCode) : base(message, "NTF", statusCode) { }

        public class NotFound : NotificationException
        {
            public NotFound() : base($"Not found notification with given id", HttpStatusCode.NotFound) { }
        }
    }
}
