using MediatR;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Handlers.Internal;

namespace WorkTracker.Commands
{
    public record StartWorkEntryCommand(Guid EmployerId, DateTime StartTime, Guid UserId, bool IsAuthorized = false) : IEmployerAuthorizedRequest<WorkEntryModel>;
    public record EndLatestWorkEntryCommand(Guid EmployerId, DateTime EndTime, Guid UserId, bool IsAuthorized = false) : IEmployerAuthorizedRequest<WorkEntryModel>;
    public record CreateWorkEntryCommand(Guid EmployerId, DateTime StartTime, DateTime? EndTime, Guid UserId, bool IsAuthorized = false) : IEmployerAuthorizedRequest<WorkEntryModel>;
    public record UpdateWorkEntryCommand(Guid EmployerId, DateTime OldStartTime, DateTime NewStartTime, DateTime? NewEndTime, Guid UserId, bool IsAuthorized = false) : IEmployerAuthorizedRequest<WorkEntryModel>;
    public record DeleteWorkEntryCommand(Guid EmployerId, DateTime StartTime, Guid UserId, bool IsAuthorized = false) : IEmployerAuthorizedRequest<Unit>;
}
