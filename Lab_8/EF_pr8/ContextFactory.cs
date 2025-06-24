using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EF_pr8
{
    // Фабрика для создания контекстов БД
    public static class ContextFactory
    {
        // Создание контекста для реальной БД
        public static AirplaneContextBase CreateContext(InheritanceStrategy strategy)
        {
            var services = new ServiceCollection();

            services.AddDbContext<AirplaneContextBase>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StrategyDemoDb;Trusted_Connection=True;")
            );

            var provider = services.BuildServiceProvider();
            var context = provider.GetRequiredService<AirplaneContextBase>();
            context.Strategy = strategy;

            return context;
        }

        // Создание in-memory контекста для тестов
        public static AirplaneContextBase CreateInMemoryContext(InheritanceStrategy strategy)
        {
            var options = new DbContextOptionsBuilder<AirplaneContextBase>()
                .UseInMemoryDatabase($"InMemory_{strategy}_{Guid.NewGuid()}")
                .Options;

            var context = new AirplaneContextBase(options);
            context.Strategy = strategy;

            return context;
        }
    }
}