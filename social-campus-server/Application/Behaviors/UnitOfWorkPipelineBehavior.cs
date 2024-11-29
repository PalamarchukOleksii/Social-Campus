using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Shared;
using MediatR;
using System.Transactions;

namespace Application.Behaviors
{
    public class UnitOfWorkPipelineBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!typeof(TRequest).GetInterfaces().Any(i => i == typeof(ICommand)))
            {
                return await next();
            }

            using (var transactionScope = new TransactionScope())
            {
                var response = await next();

                await unitOfWork.SaveChangesAsync(cancellationToken);

                transactionScope.Complete();

                return response;
            }
        }
    }
}
