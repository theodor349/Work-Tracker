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

    public class GetEmployerListHandler : IRequestHandler<GetEmployerListQuery, IEnumerable<EmployerModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployerListHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EmployerModel>> Handle(GetEmployerListQuery request, CancellationToken cancellationToken)
        {
            _unitOfWork.StartTransaction();
            var data = await _unitOfWork.Employers.GetAllAsync();
            _unitOfWork.CommitTransaction();

            data = data.Where(x => x.UserId == request.UserId).ToList();
            return data;
        }
    }

    public class GetEmplyerByIdHandler : IRequestHandler<GetEmployerByIdQuery, EmployerModel>
    {
        private readonly IMediator _mediator;

        public GetEmplyerByIdHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<EmployerModel> Handle(GetEmployerByIdQuery request, CancellationToken cancellationToken)
        {
            var emplyers = await _mediator.Send(new GetEmployerListQuery(request.UserId));
            return emplyers.FirstOrDefault(x => x.Id == request.Id);
        }
    }

    public class GetEmployerBalanceQueryHandler : GenericEmployerAuthorizedRequestHandler<GetEmployerBalanceQuery, EmployerBalanace, IUnitOfWork>
    {
        public GetEmployerBalanceQueryHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override async Task<EmployerBalanace> HandleTransaction(GetEmployerBalanceQuery request, CancellationToken cancellationToken)
        {
            await RequireAccessToEmployer(request);
            var data = await _unitOfWork.Employers.GetEmployerBalanceBeforeAsync(request.EmployerId, request.BeforeDate);
            return data;
        }
    }
}
