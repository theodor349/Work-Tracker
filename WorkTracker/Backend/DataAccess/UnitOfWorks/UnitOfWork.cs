using DataAccess.Implementations;

namespace DataAccess.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    public IEmployerRepository Employers { get; }
    public IWorkEntryRepository WorkEntires { get; }
    public IInvoiceRepository Invoices { get; }

    public UnitOfWork(IEmployerRepository employers, IWorkEntryRepository workEntires, IInvoiceRepository statements)
    {
        Employers = employers;
        WorkEntires = workEntires;
        Invoices = statements;
    }

    public void StartTransaction()
    {
        Employers.StartTransaction();
        WorkEntires.StartTransaction();
        Invoices.StartTransaction();
    }

    public void CommitTransaction()
    {
        Employers.CommitTransaction();
        WorkEntires.CommitTransaction();
        Invoices.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        Employers.RollbackTransaction();
        WorkEntires.RollbackTransaction();
        Invoices.RollbackTransaction();
    }
}
