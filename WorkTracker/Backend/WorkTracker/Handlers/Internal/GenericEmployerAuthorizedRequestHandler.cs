using DataAccess.UnitOfWorks;
using MediatR;

namespace WorkTracker.Handlers.Internal
{

    public abstract class GenericEmployerAuthorizedRequestHandler<TRequest, TResponse, TUnitOfWork> : GenericRequestHandler<TRequest, TResponse, TUnitOfWork> where TRequest : IEmployerAuthorizedRequest<TResponse> where TUnitOfWork : IUnitOfWork
    {
        protected GenericEmployerAuthorizedRequestHandler(TUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        public async override Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {

            _unitOfWork.StartTransaction();
            var data = await HandleTransaction(request, cancellationToken);
            _unitOfWork.CommitTransaction();
            return data;
        }

        internal async Task RequireAccessToEmployer(TRequest request)
        {
            if (!request.IsAuthorized)
                await RequireAccessToEmployer(request.EmployerId, request.UserId);
        }
    }
}
