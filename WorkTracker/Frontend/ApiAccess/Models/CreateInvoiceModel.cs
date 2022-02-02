using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAccess.Models
{
    public class CreateInvoiceModel 
    { 
        public Guid EmployerId { get; set; } 
        public int Year { get; set; } 
        public int Month { get; set;  } 
        public int MaxMonthlyHours { get; set; } 
        public int MaxMonthlyMinutes { get; set; }
        public int ExtraHours { get; set; } 
        public int ExtraMinutes { get; set; }
        public bool ShouldAddInvoice { get; set; }

        public CreateInvoiceModel(Guid employerId, int year, int month, int maxMonthlyHours, int maxMonthlyMinutes, int extraHours, int extraMinutes, bool shouldAddInvoice)
        {
            EmployerId = employerId;
            MaxMonthlyHours = maxMonthlyHours;
            MaxMonthlyMinutes = maxMonthlyMinutes;
            Year = year;
            Month = month;
            ExtraHours = extraHours;
            ExtraHours = extraMinutes;
            ShouldAddInvoice = shouldAddInvoice;
        }
    }
}
