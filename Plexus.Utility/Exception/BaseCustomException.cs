using System;
using System.Net;
using System.Text.Json.Serialization;

namespace Plexus.Utility.Exception
{
    public abstract class BaseCustomException : System.Exception
    {
        public string Prefix { get; }

        public HttpStatusCode StatusCode { get; set; }

        public string Code { get => $"{Prefix}{(int)StatusCode}"; }

        public BaseCustomException(string message, string prefix, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            Prefix = prefix;
            StatusCode = statusCode;
        }
    }
}

