using DataAccess.UnitOfWorks;
using MediatR;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Commands;
using WorkTracker.Queries;

namespace WorkTracker.Handlers
{
    public class CreateEmployerCommandHandler : IRequestHandler<CreateEmployerCommand, EmployerModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateEmployerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<EmployerModel> Handle(CreateEmployerCommand request, CancellationToken cancellationToken)
        {
            var model = new EmployerModel(Guid.NewGuid(), request.Name, request.UserId);

            _unitOfWork.StartTransaction();
            _unitOfWork.Employers.CreateAsync(model);
            _unitOfWork.CommitTransaction();

            return Task.FromResult(model);
        }
    }

    public class UpdateEmployerCommandHandler : IRequestHandler<UpdateEmployerCommand, EmployerModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateEmployerCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<EmployerModel> Handle(UpdateEmployerCommand request, CancellationToken cancellationToken)
        {
            var model = new EmployerModel(request.Id, request.Name, request.UserId);

            _unitOfWork.StartTransaction();
            bool isAuthorized = await _mediator.Send(new HasAccessToEmployerQuery(request.Id, request.UserId));
            if (isAuthorized)
                await _unitOfWork.Employers.UpdateAsync(model);
            else
                throw new UnauthorizedAccessException("User: " + request.UserId + " is not authorized for employer: " + request.Id);
            _unitOfWork.CommitTransaction();
            return model;
        }
    }

    public class DeleteEmployerCommandHandler : IRequestHandler<DeleteEmployerCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeleteEmployerCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<int> Handle(DeleteEmployerCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.StartTransaction();
            int rows;
            bool isAuthorized = await _mediator.Send(new HasAccessToEmployerQuery(request.Id, request.UserId));
            if (isAuthorized)
                rows = await _unitOfWork.Employers.DeleteAsync(request.Id);
            else
                throw new UnauthorizedAccessException("User: " + request.UserId + " is not authorized for employer: " + request.Id);
            _unitOfWork.CommitTransaction();
            return rows;
        }
    }
}
