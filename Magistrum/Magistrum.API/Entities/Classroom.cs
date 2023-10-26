using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Magistrum.API.Entities;

[Table("Classroom")]
public class Classroom
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [Column("Degree")]
    public required string Degree { get; set; }

    public virtual ICollection<Student>? Students { get; set; }
}