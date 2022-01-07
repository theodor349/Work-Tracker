using System.ComponentModel.DataAnnotations;

namespace Shared.Models.DTOs.WorkEntry;

public class CreateWorkEntryDto
{
    [Required]
    public Guid EmployerId { get; set; }
    [Required]
    public DateTime Start { get; set; }
    [Required]
    public DateTime End { get; set; }
}
