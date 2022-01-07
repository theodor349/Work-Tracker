using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DTOs.WorkEntry;

public class StartWorkEntryDto
{
    [Required]
    public Guid EmployerId { get; set; }
    [Required]
    public DateTime Start { get; set; }
}
