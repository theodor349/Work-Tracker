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
    public class GetInvoicesBetweenQueryHandler : GenericEmployerAuthorizedRequestHandler<GetInvoicesBetweenQuery, IReadOnlyList<InvoiceModel>, IUnitOfWork>
    {
        public GetInvoicesBetweenQueryHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override async Task<IReadOnlyList<InvoiceModel>> HandleTransaction(GetInvoicesBetweenQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Invoices.GetInvoicesBetweenAsync(request.EmployerId, request.StartDate, request.EndDate);
            return data;
        }
    }

    public class GetInvoicesCreatedBetweenQueryHandler : GenericEmployerAuthorizedRequestHandler<GetInvoicesCreatedBetweenQuery, IReadOnlyList<InvoiceModel>, IUnitOfWork>
    {
        public GetInvoicesCreatedBetweenQueryHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override async Task<IReadOnlyList<InvoiceModel>> HandleTransaction(GetInvoicesCreatedBetweenQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Invoices.GetInvoicesCreatedBetweenAsync(request.EmployerId, request.StartDate, request.EndDate);
            return data;
        }
    }
}
