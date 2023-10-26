using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Magistrum.API.Entities;

namespace Magistrum.API.Data;

public partial class MagistrumContext : IdentityDbContext
{
    public MagistrumContext()
    {
    }

    public MagistrumContext(DbContextOptions<MagistrumContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build().GetConnectionString("MagistrumConnectionString");

        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // IdentityDbContext needs this call
        
        OnModelCreatingPartial(modelBuilder);
        
        modelBuilder.Entity<Schedule>()
            .Property(e => e.StartTime)
            .HasColumnType("time without time zone");

        modelBuilder.Entity<Schedule>()
            .Property(e => e.EndTime)
            .HasColumnType("time without time zone");
    }

    public virtual DbSet<Director> Directors { get; set; } = null!;
    public virtual DbSet<Classroom> Classrooms { get; set; } = null!;
    public virtual DbSet<Student> Students { get; set; } = null!;
    public virtual DbSet<Subject> Subjects { get; set; } = null!;
    public virtual DbSet<Professor> Professors { get; set; } = null!;
    public virtual DbSet<Schedule> Schedules { get; set; } = null!;

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
