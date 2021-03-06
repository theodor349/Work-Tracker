using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DTOs
{
    public class EmployerDisplayModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public TimeSpan TimeThisMonth { get; set; }
        public bool IsStarted => StartTime is not null;
        public EmployerBalanace Balance { get; set; }
    }
}
