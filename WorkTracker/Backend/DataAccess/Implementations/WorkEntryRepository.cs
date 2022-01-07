using DataAccess.Internal;
using DataAccess.Internal.Databases;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Implementations;


public class WorkEntryRepository : GenericRepository<WorkEntryModel, Tuple<Guid, DateTime>>, IWorkEntryRepository
{
    public WorkEntryRepository(IWorkTranckingDB sqlDataAccess) :
        base(
            sqlDataAccess, "WorkEntry",
            StoredProcedure.spWorkEntry_Create, StoredProcedure.spWorkEntry_Update, StoredProcedure.spWorkEntry_Delete
        )
    {
    }

    public override Task<int> CreateAsync(WorkEntryModel entity)
    {
        var data = _sql.SaveDataInTransaction(_create, new { entity.EmployerId, entity.StartTime, entity.EndTime });
        return Task.FromResult(data);
    }

    [Obsolete("Use 'UpdateAsync(WorkEntryModel entity, DateTime oldStarTime)' instead!")]
    public override Task<int> UpdateAsync(WorkEntryModel entity)
    {
        throw new NotImplementedException("This method should not be used. Use 'UpdateAsync(WorkEntryModel entity, DateTime oldStarTime)' instead!");
    }

    public override Task<int> DeleteAsync(Tuple<Guid, DateTime> id)
    {
        var data = _sql.DeleteDataInTransaction(_delete, new { employerId = id.Item1, startTime = id.Item2 });
        return Task.FromResult(data);
    }

    public Task<int> UpdateAsync(WorkEntryModel entity, DateTime oldStartTime)
    {
        var data = _sql.UpdateDataInTransaction(_update, new
        {
            entity.EmployerId,
            oldStartTime,
            newStartTime = entity.StartTime,
            newEndTime = entity.EndTime,
        });
        return Task.FromResult(data);
    }

    public Task<WorkEntryModel?> GetLatestAsync(Guid employerId)
    {
        var data = _sql.LoadDataInTransaction<WorkEntryModel, object>(StoredProcedure.spWorkEntry_GetLatest, new { employerId });
        return Task.FromResult(data.FirstOrDefault());
    }

    public Task<IReadOnlyList<WorkEntryModel>> GetAllByUserIdAsync(Guid employerId)
    {
        var data = _sql.LoadDataInTransaction<WorkEntryModel, object>(StoredProcedure.spWorkEntry_GetAllByEmployerId, new { employerId });
        return Task.FromResult(data);
    }

    internal override bool Compare(WorkEntryModel model, Tuple<Guid, DateTime> id)
    {
        return model.EmployerId == id.Item1 && model.StartTime == id.Item2;
    }
}
