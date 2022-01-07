using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models;

public class EmployerModel
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public Guid UserId { get; set; }

    public EmployerModel(Guid id, string name, Guid userId)
    {
        Id = id;
        Name = name;
        UserId = userId;
    }
}
