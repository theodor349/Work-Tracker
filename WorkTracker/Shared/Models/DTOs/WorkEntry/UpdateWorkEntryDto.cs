using System.ComponentModel.DataAnnotations;

namespace Shared.Models.DTOs.WorkEntry;

public class UpdateWorkEntryDto
{
    [Required]
    public DateTime OldStartTime { get; set; }
    [Required]
    public DateTime NewStartTime { get; set; }
    [Required]
    public DateTime NewEndTime { get; set; }
}
