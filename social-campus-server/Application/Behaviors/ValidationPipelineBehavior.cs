using Domain.Shared;
using FluentValidation;
using MediatR;

namespace Application.Behaviors
{
    public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!validators.Any())
            {
                return await next();
            }

            Error[] errors = await Task.WhenAll(validators
                .Select(validator => validator.ValidateAsync(request, cancellationToken)))
                .ContinueWith(task => task.Result
                    .SelectMany(validationResult => validationResult.Errors)
                    .Where(validationFailure => validationFailure is not null)
                    .Select(failure => new Error(
                        failure.PropertyName,
                        failure.ErrorMessage))
                    .Distinct()
                    .ToArray());

            if (errors.Length != 0)
            {
                return CreateValidationResult<TResponse>(errors);
            }

            return await next();
        }

        private static TResult CreateValidationResult<TResult>(Error[] errors)
            where TResult : Result
        {
            if (typeof(TResult) == typeof(Result))
            {
                return (ValidationResult.WithErrors(errors) as TResult)!;
            }

            object validationResult = typeof(ValidationResult<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
                .GetMethod(nameof(ValidationResult.WithErrors))!
                .Invoke(null, [errors])!;

            return (TResult)validationResult;
        }
    }
}
