using Application.Abstractions.Data;
using MediatR;
using System.Transactions;

namespace Application.Behaviors
{
    public class UnitOfWorkPipelineBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!nameof(TRequest).EndsWith("Command"))
            {
                return await next();
            }

            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var response = await next();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            transactionScope.Complete();

            return response;
        }
    }
}
