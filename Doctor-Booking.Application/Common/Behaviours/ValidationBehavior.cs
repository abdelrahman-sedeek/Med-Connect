using FluentValidation;
using MediatR;
using Doctor_Booking.Application.ViewModels;

namespace Doctor_Booking.Application.Common.Behaviours
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull where TResponse : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // If no validators registered, skip validation
            if (!_validators.Any())
            {
                return await next();
            }

            // Create validation context
            var context = new ValidationContext<TRequest>(request);

            // Run all validators and collect failures
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var failures = validationResults
                .Where(r => !r.IsValid)
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            // If no failures, proceed to handler
            if (!failures.Any())
            {
                return await next();
            }

            // Build error list
            var errors = failures
                .Select(f => (object)new
                {
                    PropertyName = f.PropertyName,
                    Message = f.ErrorMessage
                })
                .ToList();

            // If caller expects ResponseViewModel<T>, construct a matching ResponseViewModel<T>
            var responseType = typeof(TResponse);
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(ResponseViewModel<>))
            {
                var genericArg = responseType.GetGenericArguments()[0];
                var concreteResponseType = typeof(ResponseViewModel<>).MakeGenericType(genericArg);

                // Create instance and set properties via reflection
                var instance = Activator.CreateInstance(concreteResponseType)!;

                // Data = null (default)
                var propData = concreteResponseType.GetProperty(nameof(ResponseViewModel<object>.Data));
                propData?.SetValue(instance, null);

                // IsSucsess = false
                var propIsSuccess = concreteResponseType.GetProperty(nameof(ResponseViewModel<object>.IsSucsess));
                propIsSuccess?.SetValue(instance, false);

                // Status = 422
                var propStatus = concreteResponseType.GetProperty(nameof(ResponseViewModel<object>.Status));
                propStatus?.SetValue(instance, 422);

                // Message
                var propMessage = concreteResponseType.GetProperty(nameof(ResponseViewModel<object>.Message));
                propMessage?.SetValue(instance, "Validation failed. Please check the errors.");

                // Errors
                var propErrors = concreteResponseType.GetProperty(nameof(ResponseViewModel<object>.Errors));
                propErrors?.SetValue(instance, errors);

                return (TResponse)instance;
            }

            // If TResponse is not ResponseViewModel<>, fall back to throwing validation exception
            throw new ValidationException(failures);
        }
    }
}