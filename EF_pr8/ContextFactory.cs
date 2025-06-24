using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EF_pr8
{
    public enum StrategyType { TPH, TPT, TPC }

    public static class ContextFactory
    {
        public static AirplaneContextBase CreateContext(StrategyType strategy)
        {
            var services = new ServiceCollection();

            services.AddDbContext<AirplaneContextBase>(options =>
            {
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StrategyDemoDb;Trusted_Connection=True;");
            });

            var provider = services.BuildServiceProvider();
            var context = provider.GetRequiredService<AirplaneContextBase>();

            context.Strategy = strategy switch
            {
                StrategyType.TPH => InheritanceStrategy.TPH,
                StrategyType.TPT => InheritanceStrategy.TPT,
                StrategyType.TPC => InheritanceStrategy.TPC,
                _ => throw new ArgumentException("Unknown strategy")
            };

            return context;
        }
    }
}