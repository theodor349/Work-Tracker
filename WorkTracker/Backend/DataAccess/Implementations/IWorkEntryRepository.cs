using DataAccess.Internal;
using Shared.Models;

namespace DataAccess.Implementations;

public interface IWorkEntryRepository : IGenericRepository<WorkEntryModel, Tuple<Guid, DateTime>>
{
    Task<IReadOnlyList<WorkEntryModel>> GetAllByUserIdAsync(Guid employerId);
    Task<WorkEntryModel?> GetLatestAsync(Guid userId);
    Task<int> UpdateAsync(WorkEntryModel entity, DateTime oldStarTime);
    [Obsolete("Use 'UpdateAsync(WorkEntryModel entity, DateTime oldStarTime)' instead!")]
    Task<int> UpdateAsync(WorkEntryModel entity);
}
