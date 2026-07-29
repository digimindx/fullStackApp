using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Core.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicit column mapping to match your SQL schema lowercase columns
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees", "HR");

                entity.Property(e => e.Email)
                    .HasColumnName("email");

                entity.Property(e => e.Username)
                    .HasColumnName("username");

                entity.Property(e => e.Password)
                    .HasColumnName("password");
            });
        }
    }
}