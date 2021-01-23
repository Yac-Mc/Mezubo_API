using Mezubo_Api.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Mezubo_Api.Handler
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("---------------------------------------------------------------------------------------------------{0} Error: {1}{0}", Environment.NewLine, ex));
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new GenericResponseEntity<string>()
            {
                IsSuccessful = false,
                StatusCode = context.Response.StatusCode,
                Message = "Excepción: Ha ocurrido un error. Por favor intentelo más tarde."
            }.ToString());
        }
    }
}
