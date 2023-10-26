
using System.ComponentModel.DataAnnotations;

namespace Magistrum.API.ViewModels;

public class ScheduleCreateViewModel
{

    [Required]
    [DataType(DataType.Time)]
    public required TimeOnly StartTime { get; set; }

    [Required]
    [DataType(DataType.Time)]
    public required TimeOnly EndTime { get; set; }
}