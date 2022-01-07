using DataAccess.UnitOfWorks;
using MediatR;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Commands;
using WorkTracker.Handlers.Internal;
using WorkTracker.Queries;

namespace WorkTracker.Handlers
{
    public class StartWorkEntryCommandHandler : GenericEmployerAuthorizedRequestHandler<StartWorkEntryCommand, WorkEntryModel, IUnitOfWork>
    {
        public StartWorkEntryCommandHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override async Task<WorkEntryModel> HandleTransaction(StartWorkEntryCommand request, CancellationToken cancellationToken)
        {
            await RequireAccessToEmployer(request);
            var data = await _mediator.Send(new CreateWorkEntryCommand(request.EmployerId, request.StartTime, null, request.UserId, true));
            return data;
        }
    }

    public class EndLatestWorkEntryCommandHandler : GenericEmployerAuthorizedRequestHandler<EndLatestWorkEntryCommand, WorkEntryModel, IUnitOfWork>
    {
        public EndLatestWorkEntryCommandHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override async Task<WorkEntryModel> HandleTransaction(EndLatestWorkEntryCommand request, CancellationToken cancellationToken)
        {
            await RequireAccessToEmployer(request);
            var data = await _mediator.Send(new GetLatestWorkEntryQuery(request.EmployerId, request.UserId, true));
            RequireNoneEndedModel(request, data);
            data.EndTime = request.EndTime;
            await _mediator.Send(new UpdateWorkEntryCommand(data.EmployerId, data.StartTime, data.StartTime, data.EndTime, request.UserId, true));
            return data;
        }

        private static void RequireNoneEndedModel(EndLatestWorkEntryCommand request, WorkEntryModel? data)
        {
            if (data is null)
                throw new ArgumentNullException("No WorkEntry was found for EmployerId: " + request.EmployerId);
            if (data.EndTime is not null)
                throw new ArgumentNullException("All WorkEntries has been ended for EmployerId: " + request.EmployerId);
        }
    }

    public class CreateWorkEntryCommandCommandHandler : GenericEmployerAuthorizedRequestHandler<CreateWorkEntryCommand, WorkEntryModel, IUnitOfWork>
    {
        public CreateWorkEntryCommandCommandHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override async Task<WorkEntryModel> HandleTransaction(CreateWorkEntryCommand request, CancellationToken cancellationToken)
        {
            await RequireAccessToEmployer(request);
            var data = new WorkEntryModel(request.EmployerId, request.StartTime, request.EndTime);
            await _unitOfWork.WorkEntires.CreateAsync(data);
            return data;
        }
    }

    public class UpdateWorkEntryCommandCommandHandler : GenericEmployerAuthorizedRequestHandler<UpdateWorkEntryCommand, WorkEntryModel, IUnitOfWork>
    {
        public UpdateWorkEntryCommandCommandHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override async Task<WorkEntryModel> HandleTransaction(UpdateWorkEntryCommand request, CancellationToken cancellationToken)
        {
            await RequireAccessToEmployer(request);
            var data = await _mediator.Send(new GetWorkEntryByStartTimeQuery(request.EmployerId, request.OldStartTime, request.UserId, true));
            data.StartTime = request.NewStartTime;
            data.EndTime = request.NewEndTime;
            await _unitOfWork.WorkEntires.UpdateAsync(data, request.OldStartTime);
            return data;
        }
    }

    public class DeleteWorkEntryCommandCommandHandler : GenericEmployerAuthorizedRequestHandler<DeleteWorkEntryCommand, Unit, IUnitOfWork>
    {
        public DeleteWorkEntryCommandCommandHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override async Task<Unit> HandleTransaction(DeleteWorkEntryCommand request, CancellationToken cancellationToken)
        {
            await RequireAccessToEmployer(request);
            await _unitOfWork.WorkEntires.DeleteAsync(new Tuple<Guid, DateTime>(request.EmployerId, request.StartTime));
            return new Unit();
        }
    }

    public class ExtractTimeCommandHandler : GenericEmployerAuthorizedRequestHandler<CreateInvoiceCommand, Unit, IUnitOfWork>
    {
        public ExtractTimeCommandHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override Task<Unit> HandleTransaction(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Registering " + request.TotalTime.TotalHours + " hours for month " + request.CreationDate.ToString("MMMM"));
            return Task.FromResult(new Unit());
        }
    }
}
