using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Magistrum.API.Entities;

[Table("Schedule")]
public class Schedule
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    // Without time zone
    [Required]
    [DataType(DataType.Time)]
    [Column("startTime")]
    public required TimeOnly StartTime { get; set; }

    [Required]
    [DataType(DataType.Time)]
    [Column("endTime")]
    public required TimeOnly EndTime { get; set; }
}