using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HelpDesk4GL.Models;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk4GL.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Reclamation>? Reclamations { get; set; }
    public DbSet<ActionReclamation>? Actions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Reclamations)
            .WithOne(r => r.user)
            .HasForeignKey(r => r.UserId);
            // .IsRequired();

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable(name: "User");
        });
    }


}
