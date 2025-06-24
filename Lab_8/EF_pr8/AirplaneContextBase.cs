using Microsoft.EntityFrameworkCore;

namespace EF_pr8
{
    /// <summary>
    /// Базовый контекст БД с поддержкой стратегий наследования
    /// Бизнес-задача: Обеспечить гибкое отображение иерархии классов на БД
    /// </summary>
    public class AirplaneContextBase : DbContext
    {
        public InheritanceStrategy Strategy { get; set; } = InheritanceStrategy.TPH;

        public DbSet<Plane> Planes { get; set; }
        public DbSet<ElectricalPlane> ElectricalPlanes { get; set; }
        public DbSet<NuclearPlane> NuclearPlanes { get; set; }
        public DbSet<SteamPlane> SteamPlanes { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }

        // Конструктор для DI
        public AirplaneContextBase(DbContextOptions<AirplaneContextBase> options) : base(options){}

        // Конструктор для ручной конфигурации
        public AirplaneContextBase(){}

        /// <summary>
        /// Конфигурация подключения к БД
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StrategyDemoDb;Trusted_Connection=True;");
            }
        }

        /// <summary>
        /// Конфигурация модели данных и стратегий наследования
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация производителя
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

            // Применение стратегии наследования
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

        /// <summary>
        /// Конфигурация TPH (Table Per Hierarchy)
        /// </summary>
        private void ConfigureTph(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plane>()
                .HasDiscriminator<string>("PlaneType")
                .HasValue<ElectricalPlane>("Electrical")
                .HasValue<NuclearPlane>("Nuclear")
                .HasValue<SteamPlane>("Steam");
        }

        /// <summary>
        /// Конфигурация TPT (Table Per Type)
        /// </summary>
        private void ConfigureTpt(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plane>().ToTable("Planes");
            modelBuilder.Entity<ElectricalPlane>().ToTable("ElectricalPlanes");
            modelBuilder.Entity<NuclearPlane>().ToTable("NuclearPlanes");
            modelBuilder.Entity<SteamPlane>().ToTable("SteamPlanes");
        }

        /// <summary>
        /// Конфигурация TPC (Table Per Concrete class)
        /// </summary>
        private void ConfigureTpc(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plane>().UseTpcMappingStrategy();
            modelBuilder.Entity<ElectricalPlane>().ToTable("ElectricalPlanes");
            modelBuilder.Entity<NuclearPlane>().ToTable("NuclearPlanes");
            modelBuilder.Entity<SteamPlane>().ToTable("SteamPlanes");
        }
    }

    /// <summary>
    /// Стратегии наследования
    /// </summary>
    public enum InheritanceStrategy { TPH, TPT, TPC }
}