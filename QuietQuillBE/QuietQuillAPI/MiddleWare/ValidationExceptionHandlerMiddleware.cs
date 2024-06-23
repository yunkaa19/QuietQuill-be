using FluentValidation;
using System.Text.Json;
using Domain.Exceptions.Base; // Ensure you include the namespace where NotFoundException is located

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
        catch (NotFoundException ex) // Handling all not found exceptions derived from NotFoundException
        {
            context.Response.StatusCode = 404; // Not Found
            context.Response.ContentType = "application/json";
            var response = new
            {
                error = "NotFound",
                message = ex.Message
            };
            var responseJson = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(responseJson);
        }
        catch (Exception ex) // Generic exception handler for unexpected errors
        {
            context.Response.StatusCode = 500; // Internal Server Error
            context.Response.ContentType = "application/json";
            var response = new
            {
                error = "InternalServerError",
                message = "An unexpected error occurred while processing your request."
            };
            var responseJson = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(responseJson);
        }
    }
}
