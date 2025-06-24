using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EF_pr7
{
    // Контекст базы данных для предметной области
    public class AirplaneContext : DbContext
    {
        public DbSet<Plane> Planes { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }

        public AirplaneContext(DbContextOptions<AirplaneContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация Manufacturer
            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Name).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Address).HasMaxLength(200);
                entity.Property(m => m.IsAChildCompany).IsRequired();
            });

            // Конфигурация Plane
            modelBuilder.Entity<Plane>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.SerialNumber).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Model).IsRequired().HasMaxLength(50);
                entity.Property(p => p.PlaneCode).IsRequired().HasMaxLength(20);
                entity.Property(p => p.EngineType).IsRequired();

                // Связь один-ко-многим
                entity.HasOne(p => p.Manufacturer)
                      .WithMany(m => m.Planes)
                      .HasForeignKey(p => p.ManufacturerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}