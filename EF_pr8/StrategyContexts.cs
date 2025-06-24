using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EF_pr8
{
    // Задание 1: TPT-стратегия (Table Per Type)
    public class TptContext : AirplaneContextBase
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPT конфигурация
            modelBuilder.Entity<Plane>().ToTable("Planes");
            modelBuilder.Entity<ElectricalPlane>().ToTable("ElectricalPlanes");
            modelBuilder.Entity<NuclearPlane>().ToTable("NuclearPlanes");
            modelBuilder.Entity<SteamPlane>().ToTable("SteamPlanes");
        }
    }

    // Задание 2: TPC-стратегия (Table Per Concrete Class)
    public class TpcContext : AirplaneContextBase
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPC конфигурация
            modelBuilder.Entity<Plane>().UseTpcMappingStrategy();

            modelBuilder.Entity<ElectricalPlane>().ToTable("ElectricalPlanes")
                .Property(e => e.BatteryCapacity).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<NuclearPlane>().ToTable("NuclearPlanes")
                .Property(n => n.FuelRodLife).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SteamPlane>().ToTable("SteamPlanes")
                .Property(s => s.BoilerPressure).HasColumnType("decimal(18,2)");
        }
    }

    // Задание 3: TPH-стратегия (Table Per Hierarchy)
    public class TphContext : AirplaneContextBase
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPH конфигурация
            modelBuilder.Entity<Plane>()
                .HasDiscriminator<string>("PlaneType")
                .HasValue<ElectricalPlane>("Electrical")
                .HasValue<NuclearPlane>("Nuclear")
                .HasValue<SteamPlane>("Steam");

            modelBuilder.Entity<Plane>()
                .Property("PlaneType")
                .HasMaxLength(20);
        }
    }
}