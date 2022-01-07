using DataAccess.Implementations;
using DataAccess.Internal.Databases;
using DataAccess.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class ConfigureServices
    {
        public static void AddDataAccess(this IServiceCollection services)
        {
            // Scoped means that the same service will be used throughout a requests lifetime: https://stackoverflow.com/questions/38138100/addtransient-addscoped-and-addsingleton-services-differences
            services.AddScoped<IWorkTranckingDB, WorkTrackingDB>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IEmployerRepository, EmployerRepository>();
            services.AddTransient<IWorkEntryRepository, WorkEntryRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}