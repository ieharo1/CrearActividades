using System.Text.Json;
using EnterpriseMediaVault.Application.Common;
using FluentValidation;

namespace EnterpriseMediaVault.API.Middleware;

public sealed class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            var errors = ex.Errors.Select(e => e.ErrorMessage).ToArray();
            await context.Response.WriteAsync(JsonSerializer.Serialize(ApiResponse<object>.Fail("Error de validación", errors)));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(ApiResponse<object>.Fail("Error interno del servidor", "UNHANDLED_EXCEPTION")));
        }
    }
}
