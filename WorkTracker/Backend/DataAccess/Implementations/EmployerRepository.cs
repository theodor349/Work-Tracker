using DataAccess.Internal;
using DataAccess.Internal.Databases;
using Shared.Models;

namespace DataAccess.Implementations;

public class EmployerRepository : GenericRepository<EmployerModel, Guid>, IEmployerRepository
{
    public EmployerRepository(IWorkTranckingDB sqlDataAccess) :
        base(
            sqlDataAccess, "Employer",
            StoredProcedure.spEmployer_Create, StoredProcedure.spEmployer_Update, StoredProcedure.spEmployer_Delete
            )
    {
    }

    public Task<EmployerBalanace> GetEmployerBalanceBeforeAsync(Guid employerId, DateTime beforeDate)
    {
        var data = _sql.LoadDataInTransaction<EmployerBalanace, object>(StoredProcedure.spEmployer_GetBalance, new { employerId, beforeDate });
        var res = data.FirstOrDefault();
        if (res == null)
            res = new EmployerBalanace(employerId, 0, 0);
        return Task.FromResult(res);
    }

    internal override bool Compare(EmployerModel model, Guid id)
    {
        return model.Id == id;
    }
}
