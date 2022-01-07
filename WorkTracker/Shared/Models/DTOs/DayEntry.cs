using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DTOs;

public class DayEntry
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Note { get; set; }

    public TimeSpan Duration => EndTime - StartTime;

    public DayEntry(DateTime startTime, DateTime endTime, string note = "")
    {
        StartTime = startTime;
        EndTime = endTime;
        Note = note;

        if (Duration.Days > 0)
            throw new ArgumentException("DayEnties cannot span multiple days");
    }
}
