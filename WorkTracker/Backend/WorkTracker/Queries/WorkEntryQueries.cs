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
    public record GetWorkEntryListQuery(Guid EmployerId, Guid UserId, bool IsAuthorized = false) : IEmployerAuthorizedRequest<IReadOnlyList<WorkEntryModel>>;
    public record GetWorkEntryBetweenListQuery(Guid EmployerId, Guid UserId, DateTime StartTime, DateTime EndTime, bool IsAuthorized = false) : IEmployerAuthorizedRequest<IReadOnlyList<WorkEntryModel>>;
    public record GetWorkEntryByStartTimeQuery(Guid EmployerId, DateTime StartTime, Guid UserId, bool IsAuthorized = false) : IEmployerAuthorizedRequest<WorkEntryModel>;
    public record GetLatestWorkEntryQuery(Guid EmployerId, Guid UserId, bool IsAuthorized = false) : IEmployerAuthorizedRequest<WorkEntryModel>;
}
