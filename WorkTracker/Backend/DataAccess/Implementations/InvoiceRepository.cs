using DataAccess.DB_Models;
using DataAccess.Internal;
using DataAccess.Internal.Databases;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Implementations
{
    internal class InvoiceRepository : GenericRepository<InvoiceModel, Tuple<Guid, DateTime>>, IInvoiceRepository
    {
        public InvoiceRepository(IWorkTranckingDB sqlDataAccess) :
            base(
                sqlDataAccess,
                "Invoice",
                StoredProcedure.spInvoice_Create,
                StoredProcedure.spInvoice_Update,
                StoredProcedure.spInvoice_Delete)
        {
        }

        public override Task<int> CreateAsync(InvoiceModel entity)
        {
            var data = _sql.SaveDataInTransaction(_create, (InvoiceDBModel)entity);
            return Task.FromResult(data);
        }

        public override Task<int> DeleteAsync(Tuple<Guid, DateTime> id)
        {
            var data = _sql.DeleteDataInTransaction(_delete, new { employerId = id.Item1, creationTime = id.Item2 });
            return Task.FromResult(data);
        }

        public override Task<int> UpdateAsync(InvoiceModel entity)
        {
            var data = _sql.UpdateDataInTransaction(_update, (InvoiceDBModel)entity);
            return Task.FromResult(data);
        }

        public override Task<IReadOnlyList<InvoiceModel>> GetAllAsync()
        {
            var data = _sql.LoadDataInTransaction<InvoiceDBModel, object>(StoredProcedure.spGeneric_GetAll, new { table = _tableName });
            IReadOnlyList<InvoiceModel> res = data.Cast<InvoiceModel>().ToList();
            return Task.FromResult(res);
        }

        internal override bool Compare(InvoiceModel model, Tuple<Guid, DateTime> id)
        {
            bool res = true;
            res &= model.EmployerId == id.Item1;
            res &= model.CreationDate == id.Item2;
            return res;
        }

        public Task<IReadOnlyList<InvoiceModel>> GetInvoicesBetweenAsync(Guid employerId, DateTime startDate, DateTime endDate)
        {
            var data = _sql.LoadDataInTransaction<InvoiceDBModel, object>(StoredProcedure.spInvoice_GetBetween, new { employerId, startDate, endDate });
            var data2 = new List<InvoiceModel>();
            foreach (var item in data)
            {
                data2.Add(item);
            }
            IReadOnlyList<InvoiceModel> res = data2;
            //IReadOnlyList<InvoiceModel> res = data.Cast<InvoiceModel>().ToList();
            return Task.FromResult(res);
        }

        public Task<IReadOnlyList<InvoiceModel>> GetInvoicesCreatedBetweenAsync(Guid employerId, DateTime startDate, DateTime endDate)
        {
            var data = _sql.LoadDataInTransaction<InvoiceDBModel, object>(StoredProcedure.spInvoice_GetCreatedBetween, new { employerId, startDate, endDate });
            var data2 = new List<InvoiceModel>();
            foreach (var item in data)
            {
                data2.Add(item);
            }
            IReadOnlyList<InvoiceModel> res = data2;
            //IReadOnlyList<InvoiceModel> res = data.Cast<InvoiceModel>().ToList();
            return Task.FromResult(res);
        }
    }
}
