using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class InvoiceModel
    {
        public Guid EmployerId { get; set; }
        public DateTime CreationDate { get; set; }
        public TimeSpan TotalTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public InvoiceModel(Guid employerId, DateTime creationDate, TimeSpan totalTime, DateTime startDate, DateTime endDate)
        {
            EmployerId = employerId;
            CreationDate = creationDate;
            TotalTime = totalTime;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
