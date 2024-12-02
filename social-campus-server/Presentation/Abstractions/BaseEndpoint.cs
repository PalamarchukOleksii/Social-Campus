using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Abstractions
{
    public abstract class BaseEndpoint
    {
        protected static IResult HandleFailure(Result result)
        {
            return result switch
            {
                { IsSuccess: true } => throw new InvalidOperationException("Cannot handle failure for a successful result"),
                IValidationResult validationResult =>
                    Results.BadRequest(
                        CreateProblemDetails(
                            "Validation Error",
                            StatusCodes.Status400BadRequest,
                            result.Error,
                            validationResult.Errors)),
                _ =>
                    Results.BadRequest(
                        CreateProblemDetails(
                            "Bad Request",
                            StatusCodes.Status400BadRequest,
                            result.Error))
            };
        }

        private static ProblemDetails CreateProblemDetails(
            string title,
            int status,
            Error error,
            Error[]? errors = null)
        {
            return new()
            {
                Title = title,
                Type = error.Code,
                Detail = error.Message,
                Status = status,
                Extensions = { { nameof(error), errors } }
            };
        }
    }
}
