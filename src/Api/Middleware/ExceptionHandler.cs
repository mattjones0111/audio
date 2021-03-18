namespace Api.Middleware
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Process.Aspects.Validation;

    // ReSharper disable once ClassNeverInstantiated.Global - dynamically bound
    public class ExceptionMiddleware
    {
        readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (FeatureValidationException exception)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                HttpStatusCode mostCommonCode = exception.GetMostCommonError();

                context.Response.Clear();
                context.Response.StatusCode = (int)mostCommonCode;

                await context.Response.WriteAsync(exception.Message);
            }
            catch (Exception)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                int code = (int) HttpStatusCode.InternalServerError;

                context.Response.Clear();
                context.Response.StatusCode = code;

                await context.Response.WriteAsync(
                    "An internal server error occurred.");
            }
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}