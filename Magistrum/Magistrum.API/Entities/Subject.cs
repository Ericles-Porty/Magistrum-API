using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Magistrum.API.Entities;

[Table("Subject")]
public class Subject
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [Column("Name")]
    [MaxLength(100)]
    [MinLength(3)]
    public required string Name { get; set; }

    [Required]
    [Column("Degree")]

    public required string Degree { get; set; }

    [Required]
    [Column("PeriodStart")]
    [DataType(DataType.Date)]

    public DateOnly PeriodStart { get; set; }

    [Required]
    [Column("PeriodEnd")]
    [DataType(DataType.Date)]

    public DateOnly PeriodEnd { get; set; }

    public ICollection<Professor>? Professors { get; set; }

}