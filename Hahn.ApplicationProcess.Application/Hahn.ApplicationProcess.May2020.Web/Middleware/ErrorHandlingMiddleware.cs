using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.Web.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            string result;
            var code = context.Response.StatusCode;

            if (ex is InvalidOperationException)
            {
                result = JsonConvert.SerializeObject(new { error = ex.Message });
            } 
            else
            {
                _logger.LogError(ex, "An unexpected exception has been triggered.");

                code = (int)HttpStatusCode.InternalServerError;
                result = JsonConvert.SerializeObject(new { error = "We're sorry, an unexpected error happened in the server. Please try again shortly." });
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;

            return context.Response.WriteAsync(result);
        }
    }
}
