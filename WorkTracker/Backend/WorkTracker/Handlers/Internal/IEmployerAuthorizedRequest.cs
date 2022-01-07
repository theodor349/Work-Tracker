using MediatR;

namespace WorkTracker.Handlers.Internal
{
    public interface IEmployerAuthorizedRequest<out TResponse> : IRequest<TResponse>
    {
        bool IsAuthorized { get; }
        Guid EmployerId { get; }
        Guid UserId { get; }
    }
}
