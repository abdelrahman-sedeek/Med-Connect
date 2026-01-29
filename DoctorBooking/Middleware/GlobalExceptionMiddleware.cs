using System.Net;
using System.Text.Json;
using Doctor_Booking.Application.ViewModels;
using FluentValidation;

namespace DoctorBooking.Middleware
{
    /// <summary>
    /// Handles all exceptions and converts them to ResponseViewModel format
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            ResponseViewModel<object> response;

            switch (exception)
            {
                case ValidationException validationEx:
                    // Handle FluentValidation exceptions
                    context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    var errors = validationEx.Errors
                        .Select(e => (object)new { PropertyName = e.PropertyName, Message = e.ErrorMessage })
                        .ToList();
                    response = ResponseViewModel<object>.FailureResponse(
                        message: "Validation failed. Please check the errors.",
                        status: 422,
                        errors: errors
                    );
                    break;

                case KeyNotFoundException keyNotFoundEx:
                    // Handle not found exceptions
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response = ResponseViewModel<object>.FailureResponse(
                        message: keyNotFoundEx.Message,
                        status: 404
                    );
                    break;

                case InvalidOperationException invalidOpEx:
                    // Handle invalid operation exceptions
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = ResponseViewModel<object>.FailureResponse(
                        message: invalidOpEx.Message,
                        status: 400
                    );
                    break;

                case ArgumentException argEx:
                    // Handle argument exceptions
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = ResponseViewModel<object>.FailureResponse(
                        message: argEx.Message,
                        status: 400
                    );
                    break;

                default:
                    // Handle unexpected exceptions
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var message = _environment.IsDevelopment()
                        ? exception.Message
                        : "An unexpected error occurred. Please try again later.";
                    response = ResponseViewModel<object>.FailureResponse(
                        message: message,
                        status: 500
                    );
                    break;
            }

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
