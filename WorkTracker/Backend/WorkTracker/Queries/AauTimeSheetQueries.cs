using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Handlers.Internal;

namespace WorkTracker.Queries;

public record GetAauTimeSheetQuery(Guid EmployerId, Guid UserId, DateTime StartDate, DateTime EndDate, TimeSpan MaxMonthlyHours, TimeSpan ExtraHours, bool ShouldAddInvoice, bool IsAuthorized = false) : IEmployerAuthorizedRequest<string>;
