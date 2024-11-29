using Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("[{DateTimeUtc}] Starting request {RequestName}",
                DateTime.UtcNow,
                typeof(TRequest).Name);

            Stopwatch stopwatch = Stopwatch.StartNew();
            var response = await next();
            stopwatch.Stop();

            if (response.IsFailure)
            {
                logger.LogError(
                    "[{DateTimeUtc}] Request {RequestName} failed with error: {ErrorCode}, {ErrorMessage}",
                    DateTime.UtcNow,
                    typeof(TRequest).Name,
                    response.Error.Code,
                    response.Error.Message);
            }

            logger.LogInformation(
                "[{DateTimeUtc}] Completed request {RequestName} in {ElapsedMilliseconds}ms",
                DateTime.UtcNow,
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds);

            return response;
        }
    }
}
