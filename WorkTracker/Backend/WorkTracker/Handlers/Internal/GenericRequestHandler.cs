using DataAccess.UnitOfWorks;
using MediatR;
using WorkTracker.Queries;

namespace WorkTracker.Handlers.Internal
{
    public abstract class GenericRequestHandler<TRequest, TResponse, TUnitOfWork> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse> where TUnitOfWork : IUnitOfWork
    {
        internal readonly TUnitOfWork _unitOfWork;
        internal readonly IMediator _mediator;

        public GenericRequestHandler(TUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            _unitOfWork.StartTransaction();
            var data = await HandleTransaction(request, cancellationToken);
            _unitOfWork.CommitTransaction();
            return data;
        }

        internal abstract Task<TResponse> HandleTransaction(TRequest request, CancellationToken cancellationToken);

        internal async Task RequireAccessToEmployer(Guid employerId, Guid userId)
        {
            bool isAuthorized = await _mediator.Send(new HasAccessToEmployerQuery(employerId, userId));
            if (!isAuthorized)
                throw new UnauthorizedAccessException("User: " + userId + " is not authorized for employer: " + employerId);
        }
    }
}
