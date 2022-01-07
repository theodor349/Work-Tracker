using DataAccess.UnitOfWorks;
using MediatR;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Handlers.Internal;
using WorkTracker.Queries;

namespace WorkTracker.Handlers
{
    public class GetWorkEntryListQueryHandler : GenericEmployerAuthorizedRequestHandler<GetWorkEntryListQuery, IReadOnlyList<WorkEntryModel>, IUnitOfWork>
    {
        public GetWorkEntryListQueryHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal async override Task<IReadOnlyList<WorkEntryModel>> HandleTransaction(GetWorkEntryListQuery request, CancellationToken cancellationToken)
        {
            await RequireAccessToEmployer(request);
            var data = await _unitOfWork.WorkEntires.GetAllByUserIdAsync(request.EmployerId);
            return data;
        }
    }

    public class GetWorkEntryByStartTimeQueryHandler : GenericEmployerAuthorizedRequestHandler<GetWorkEntryByStartTimeQuery, WorkEntryModel, IUnitOfWork>
    {
        public GetWorkEntryByStartTimeQueryHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override async Task<WorkEntryModel> HandleTransaction(GetWorkEntryByStartTimeQuery request, CancellationToken cancellationToken)
        {
            await RequireAccessToEmployer(request);
            var data = await _unitOfWork.WorkEntires.GetByIdAsync(new Tuple<Guid, DateTime>(request.EmployerId, request.StartTime));
            return data;
        }
    }

    public class GetLatestWorkEntryQueryHandler : GenericEmployerAuthorizedRequestHandler<GetLatestWorkEntryQuery, WorkEntryModel, IUnitOfWork>
    {
        public GetLatestWorkEntryQueryHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override async Task<WorkEntryModel> HandleTransaction(GetLatestWorkEntryQuery request, CancellationToken cancellationToken)
        {
            await RequireAccessToEmployer(request);
            var data = await _unitOfWork.WorkEntires.GetLatestAsync(request.EmployerId);
            return data;
        }
    }

    public class GetWorkEntryBetweenListQueryHandler : GenericEmployerAuthorizedRequestHandler<GetWorkEntryBetweenListQuery, IReadOnlyList<WorkEntryModel>, IUnitOfWork>
    {
        public GetWorkEntryBetweenListQueryHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override async Task<IReadOnlyList<WorkEntryModel>> HandleTransaction(GetWorkEntryBetweenListQuery request, CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(new GetWorkEntryListQuery(request.EmployerId, request.UserId, true));
            data = data.Where(x => x.StartTime.Ticks >= request.StartTime.Ticks && x.StartTime.Ticks <= request.EndTime.Ticks).ToList();
            return data;
        }
    }
}
