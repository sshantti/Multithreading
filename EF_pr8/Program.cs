using Microsoft.EntityFrameworkCore;

namespace EF_pr8
{
    class Program
    {
        static void Main(string[] args)
        {
            // Для каждой стратегии
            foreach (StrategyType strategy in Enum.GetValues(typeof(StrategyType)))
            {
                Console.WriteLine($"\n- {strategy} Strategy -");

                using var context = ContextFactory.CreateContext(strategy);

                // Пересоздаем БД
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // Заполняем данными
                DataInitializer.Initialize(context);

                // Выводим информацию
                PrintStrategyInfo(context, strategy);
            }
        }

        static void PrintStrategyInfo(AirplaneContextBase context, StrategyType strategy)
        {
            Console.WriteLine("Database Tables:");
            var tableNames = context.Database.SqlQueryRaw<string>(
                "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'"
            ).ToList();

            foreach (var table in tableNames)
            {
                Console.WriteLine($"- {table}");
            }

            // Примеры данных
            Console.WriteLine("\nElectrical Planes:");
            context.ElectricalPlanes
                .Take(2)
                .ToList()
                .ForEach(p => Console.WriteLine($"- {p.Model}: {p.BatteryCapacity}kWh"));
        }
    }
}