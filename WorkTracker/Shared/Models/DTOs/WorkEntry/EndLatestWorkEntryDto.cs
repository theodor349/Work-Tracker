using System.ComponentModel.DataAnnotations;

namespace Shared.Models.DTOs.WorkEntry;

public class EndLatestWorkEntryDto
{
    [Required]
    public Guid EmployerId { get; set; }
    [Required]
    public DateTime End { get; set; }
}
