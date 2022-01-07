using DataAccess.Internal;
using Shared.Models;

namespace DataAccess.Implementations;

public interface IEmployerRepository : IGenericRepository<EmployerModel, Guid>
{
    Task<EmployerBalanace> GetEmployerBalanceBeforeAsync(Guid employerId, DateTime beforeDate);
}
