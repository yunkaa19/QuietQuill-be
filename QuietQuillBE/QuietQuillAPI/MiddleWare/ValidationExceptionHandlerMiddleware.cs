using FluentValidation;
using System.Text.Json;

namespace QuietQuillBE.MiddleWare;

    public class ValidationExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400; // Bad Request
            context.Response.ContentType = "application/json";

            var response = new
            {
                errors = ex.Errors.Select(error => new
                {
                    propertyName = error.PropertyName,
                    errorMessage = error.ErrorMessage
                })
            };

            var responseJson = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(responseJson);
        }
    }
}
