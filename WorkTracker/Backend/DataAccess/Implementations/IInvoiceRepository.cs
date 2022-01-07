using DataAccess.Internal;
using Shared.Models;

namespace DataAccess.Implementations
{
    public interface IInvoiceRepository : IGenericRepository<InvoiceModel, Tuple<Guid, DateTime>>
    {
        Task<IReadOnlyList<InvoiceModel>> GetInvoicesBetweenAsync(Guid employerId, DateTime startDate, DateTime endDate);
    }
}