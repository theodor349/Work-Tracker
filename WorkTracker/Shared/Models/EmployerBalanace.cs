using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class EmployerBalanace
    {
        public Guid EmployerId { get; set; }
        public TimeSpan TotalWorkTime { get; set; }
        public TimeSpan TotalInvoiced { get; set; }
        public TimeSpan Balance => TotalWorkTime - TotalInvoiced;

        [JsonConstructor]
        public EmployerBalanace(Guid employerId, TimeSpan totalWorkTime, TimeSpan totalInvoiced)
        {
            EmployerId = employerId;
            TotalWorkTime = totalWorkTime;
            TotalInvoiced = totalInvoiced;
        }

        public EmployerBalanace(Guid employerId, long totalTime, long totalInvoiced)
        {
            EmployerId = employerId;
            TotalWorkTime = new TimeSpan(totalTime);
            TotalInvoiced = new TimeSpan(totalInvoiced);
        }
    }
}
