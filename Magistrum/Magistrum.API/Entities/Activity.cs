using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Magistrum.API.Entities;

[Table("Activity")]
public class Activity
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [Column("Name")]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [Column("Description")]
    [MaxLength(2500)]
    public required string Description { get; set; }

    [Required]
    [Column("SubjectId")]
    [ForeignKey("SubjectId")]
    public int SubjectId { get; set; }

    [Required]
    [Column("ProfessorId")]
    [ForeignKey("ProfessorId")]
    public int ProfessorId { get; set; }

    [Required]
    [Column("ClassroomId")]
    [ForeignKey("ClassroomId")]
    public int ClassroomId { get; set; }

    [Required]
    [Column("AchievableScore")]
    public decimal AchievableScore { get; set; }




}