using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using UPB.BusinessLogic.Managers.Exceptions;

namespace UPB.Practica_2.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try {
                await _next(httpContext);
            }
            catch (Exception ex) {
                await ProcessError(httpContext, ex);
            }
            
        }

        private Task ProcessError(HttpContext httpContext, Exception ex) { 

            string errorBodyJSON = $"{{\r\n Message = ${ex.InnerException.Message}, \r\n }}";
            return httpContext.Response.WriteAsync(errorBodyJSON);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
