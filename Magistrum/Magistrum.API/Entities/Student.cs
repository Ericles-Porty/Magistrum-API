using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Magistrum.API.Entities;

[Table("Student")]
public class Student
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [Column("Name")]
    public required string Name { get; set; }


    public virtual Classroom? Classrooms { get; set; }
}