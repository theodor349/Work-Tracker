using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DB_Models
{
    internal class InvoiceDBModel
    {
        public Guid EmployerId { get; set; }
        public DateTime CreationDate { get; set; }
        public long TotalTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public InvoiceDBModel(Guid employerId, DateTime creationDate, long totalTime, DateTime startDate, DateTime endDate) : this(employerId, creationDate, totalTime)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public InvoiceDBModel(Guid employerId, DateTime creationDate, long totalTime)
        {
            EmployerId = employerId;
            CreationDate = creationDate;
            TotalTime = totalTime;
        }

        public static implicit operator InvoiceModel(InvoiceDBModel model)
        {
            return new InvoiceModel(
                model.EmployerId, 
                model.CreationDate, 
                new TimeSpan(model.TotalTime), 
                model.StartDate, 
                model.EndDate);
        }

        public static implicit operator InvoiceDBModel(InvoiceModel model)
        {
            return new InvoiceDBModel(model.EmployerId, model.CreationDate, model.TotalTime.Ticks, model.StartDate, model.EndDate);
        }
    }
}
