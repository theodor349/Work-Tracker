using MediatR;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Commands
{
    public record CreateEmployerCommand(string Name, Guid UserId) : IRequest<EmployerModel>;
    public record UpdateEmployerCommand(string Name, Guid Id, Guid UserId) : IRequest<EmployerModel>;
    public record DeleteEmployerCommand(Guid Id, Guid UserId) : IRequest<int>;
}
