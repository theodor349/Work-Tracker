using MediatR;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Handlers.Internal;

namespace WorkTracker.Queries
{
    public record GetEmployerListQuery(Guid UserId) : IRequest<IEnumerable<EmployerModel>>;
    public record GetEmployerByIdQuery(Guid Id, Guid UserId) : IRequest<EmployerModel>;
    public record GetEmployerBalanceQuery(Guid EmployerId, Guid UserId, DateTime BeforeDate, bool IsAuthorized = false) : IEmployerAuthorizedRequest<EmployerBalanace>;
}
