using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Abstractions
{
    [ApiController]
    public abstract class ApiController(ISender sender) : ControllerBase
    {
        protected readonly ISender _sender = sender;

        protected IActionResult HandleFailure(Result result)
        {
            return result switch
            {
                { IsSuccess: true } => throw new InvalidOperationException("Cannot handle failure for a successful result"),
                IValidationResult validationResult =>
                    BadRequest(
                        CreateProblemDetails(
                            "Validation Error",
                            StatusCodes.Status400BadRequest,
                            result.Error,
                            validationResult.Errors)),
                _ =>
                    BadRequest(
                        CreateProblemDetails(
                            "Bad Request",
                            StatusCodes.Status400BadRequest,
                            result.Error))
            };
        }
        protected ActionResult<T> HandleFailure<T>(Result result)
        {
            return result switch
            {
                { IsSuccess: true } => throw new InvalidOperationException("Cannot handle failure for a successful result"),
                IValidationResult validationResult =>
                    BadRequest(
                        CreateProblemDetails(
                            "Validation Error",
                            StatusCodes.Status400BadRequest,
                            result.Error,
                            validationResult.Errors)),
                _ =>
                    BadRequest(
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
