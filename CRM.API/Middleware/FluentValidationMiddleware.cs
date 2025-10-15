using FluentValidation;

namespace CRM.API.Middleware
{
    public class FluentValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public FluentValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                
                var errors = ex.Errors.Select(error => new
                {
                    field = error.PropertyName,
                    message = error.ErrorMessage
                });
                
                await context.Response.WriteAsJsonAsync(new { errors });
            }
        }
    }

    public static class FluentValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseFluentValidationExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FluentValidationMiddleware>();
        }
    }
}