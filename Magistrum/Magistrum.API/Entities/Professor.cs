using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Magistrum.API.Entities;

[Table("Professor")]
public class Professor
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [Column("Name")]
    [MaxLength(100)]
    [MinLength(3)]
    public required string Name { get; set; }
    public virtual ICollection<Subject>? Subjects { get; set; }

}