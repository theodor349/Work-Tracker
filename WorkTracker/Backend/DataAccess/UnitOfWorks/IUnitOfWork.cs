using DataAccess.Implementations;

namespace DataAccess.UnitOfWorks;

public interface IUnitOfWork
{
    IEmployerRepository Employers { get; }
    IWorkEntryRepository WorkEntires { get; }
    IInvoiceRepository Invoices { get; }

    void StartTransaction();
    void CommitTransaction();
    void RollbackTransaction();
}
