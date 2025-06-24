using System;
using Microsoft.EntityFrameworkCore;

namespace EF_pr8
{
    public abstract class AirplaneContextBase : DbContext
    {
        public InheritanceStrategy Strategy { get; set; } = InheritanceStrategy.TPH;

        public DbSet<Plane> Planes { get; set; }
        public DbSet<ElectricalPlane> ElectricalPlanes { get; set; }
        public DbSet<NuclearPlane> NuclearPlanes { get; set; }
        public DbSet<SteamPlane> SteamPlanes { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StrategyDemoDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Базовая конфигурация
            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Name).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Address).HasMaxLength(200);
                entity.Property(m => m.IsAChildCompany).IsRequired();

                entity.HasMany(m => m.Planes)
                    .WithOne()
                    .HasForeignKey(p => p.ManufacturerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Конфигурация стратегий наследования
            switch (Strategy)
            {
                case InheritanceStrategy.TPH:
                    ConfigureTph(modelBuilder);
                    break;
                case InheritanceStrategy.TPT:
                    ConfigureTpt(modelBuilder);
                    break;
                case InheritanceStrategy.TPC:
                    ConfigureTpc(modelBuilder);
                    break;
            }
        }

        private void ConfigureTph(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plane>()
                .HasDiscriminator<string>("PlaneType")
                .HasValue<ElectricalPlane>("Electrical")
                .HasValue<NuclearPlane>("Nuclear")
                .HasValue<SteamPlane>("Steam");
        }

        private void ConfigureTpt(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plane>().ToTable("Planes");
            modelBuilder.Entity<ElectricalPlane>().ToTable("ElectricalPlanes");
            modelBuilder.Entity<NuclearPlane>().ToTable("NuclearPlanes");
            modelBuilder.Entity<SteamPlane>().ToTable("SteamPlanes");
        }

        private void ConfigureTpc(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plane>().UseTpcMappingStrategy();

            modelBuilder.Entity<ElectricalPlane>().ToTable("ElectricalPlanes");
            modelBuilder.Entity<NuclearPlane>().ToTable("NuclearPlanes");
            modelBuilder.Entity<SteamPlane>().ToTable("SteamPlanes");
        }
    }

    public enum InheritanceStrategy { TPH, TPT, TPC }
}