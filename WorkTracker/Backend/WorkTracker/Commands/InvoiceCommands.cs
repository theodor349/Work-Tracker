using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Handlers.Internal;

namespace WorkTracker.Commands
{
    public record CreateInvoiceCommand(Guid EmployerId, Guid UserId, DateTime CreationDate, DateTime StartDate, DateTime EndDate, TimeSpan TotalTime, bool IsAuthorized) : IEmployerAuthorizedRequest<Unit>;
}
