using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Handlers.Internal;

namespace WorkTracker.Queries
{
    public record GetInvoicesBetweenQuery(Guid EmployerId, Guid UserId, DateTime StartDate, DateTime EndDate, bool IsAuthorized = false) : IEmployerAuthorizedRequest<IReadOnlyList<InvoiceModel>>;
    public record GetInvoicesCreatedBetweenQuery(Guid EmployerId, Guid UserId, DateTime StartDate, DateTime EndDate, bool IsAuthorized = false) : IEmployerAuthorizedRequest<IReadOnlyList<InvoiceModel>>;
}
