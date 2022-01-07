using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class WorkEntryModel
{
    [Required]
    public Guid EmployerId { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan Duration => (TimeSpan)(EndTime is null ? TimeSpan.Zero : EndTime - StartTime);

    public WorkEntryModel(Guid employerId, DateTime startTime, DateTime? endTime)
    {
        EmployerId = employerId;
        StartTime = startTime;
        EndTime = endTime;
    }

    protected WorkEntryModel(Guid employerId, DateTime startTime)
    {
        EmployerId = employerId;
        StartTime = startTime;
    }
}
