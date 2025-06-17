using ClassLibrary;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EF_pr7
{
    public class AppDbContext : DbContext
    {
        public DbSet<Plane> Planes { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Name).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Address).HasMaxLength(200);
                entity.HasIndex(m => m.Name).IsUnique();
            });

            modelBuilder.Entity<Plane>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.SerialNumber).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Model).IsRequired().HasMaxLength(50);
                entity.Property(p => p.PlaneCode).IsRequired().HasMaxLength(10);

                entity.HasOne(p => p.Manufacturer)
                    .WithMany(m => m.Planes)
                    .HasForeignKey(p => p.ManufacturerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}