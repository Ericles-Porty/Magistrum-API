using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Magistrum.API.Entities;

namespace Magistrum.API.Entities;

[Table("Unity")]
public class Unity
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [Column("FirstDay")]
    [DataType(DataType.Date)]
    public DateOnly FirstDay { get; set; }

    [Required]
    [Column("LastDay")]
    [DataType(DataType.Date)]
    public DateOnly LastDay { get; set; }

    [Column("AverageScore")]
    public decimal AverageScore { get; set; }

    public virtual ICollection<Activity>? Activities { get; set; }
}