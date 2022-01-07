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

namespace WorkTracker.Handlers
{
    public class CreateInvoiceCommandHandler : GenericEmployerAuthorizedRequestHandler<CreateInvoiceCommand, Unit, IUnitOfWork>
    {
        public CreateInvoiceCommandHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override async Task<Unit> HandleTransaction(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            await RequireAccessToEmployer(request);
            await _unitOfWork.Invoices.CreateAsync(new InvoiceModel(request.EmployerId, request.CreationDate, request.TotalTime, request.StartDate, request.EndDate));
            return new Unit();
        }
    }
}
