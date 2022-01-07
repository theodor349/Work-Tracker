using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Queries
{
    public record HasAccessToEmployerQuery(Guid EmployerId, Guid UserId) : IRequest<bool>;
}
