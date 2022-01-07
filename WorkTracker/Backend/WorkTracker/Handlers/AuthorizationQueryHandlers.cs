using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Queries;

namespace WorkTracker.Handlers
{
    public class HasAccessToEmployerQueryHandler : IRequestHandler<HasAccessToEmployerQuery, bool>
    {
        private readonly IMediator _mediator;

        public HasAccessToEmployerQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(HasAccessToEmployerQuery request, CancellationToken cancellationToken)
        {
            var authorizedEmployers = await _mediator.Send(new GetEmployerListQuery(request.UserId));
            return authorizedEmployers.FirstOrDefault(x => x.Id == request.EmployerId) != null;
        }
    }
}
