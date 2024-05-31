using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Plexus.Utility.Exception
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next,
                                                IHostEnvironment env,
                                                ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (System.Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, System.Exception exception)
        {
            var response = context.Response;
            var isProduction = _env.IsProduction();
            response.ContentType = "application/json";

            if (exception is BaseCustomException)
            {
                var baseException = (BaseCustomException)exception;

                response.StatusCode = (int)baseException.StatusCode;

                var wrapError = new Wrapper<string>
                {
                    Code = baseException.Code,
                    Message = baseException.Message,
                    StackTrace = !isProduction ? exception.StackTrace : null,
                };

                await response.WriteAsync(JsonConvert.SerializeObject(wrapError));
            }
            else if (exception.InnerException is not null
                     && exception.InnerException is SqlException
                     && ((SqlException)exception.InnerException).Number == 547)
            {
                response.StatusCode = StatusCodes.Status409Conflict;

                var message = "Current data has been used in other table.";

                var wrapError = new Wrapper<string>
                {
                    Code = ((int)HttpStatusCode.Conflict).ToString(),
                    Message = message,
                    StackTrace = !isProduction ? exception.StackTrace : null,
                };

                await response.WriteAsync(JsonConvert.SerializeObject(wrapError));
            }
            else
            {
                response.StatusCode = 500;

                var errorResponse = new Wrapper<object>
                {
                    Code = "500C99",
                    Message = $"Unhandle Error ({exception.Message})",
                    StackTrace = !isProduction ? exception.StackTrace : null,
                    Data = exception.Data
                };

                await response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
        }
    }
}

