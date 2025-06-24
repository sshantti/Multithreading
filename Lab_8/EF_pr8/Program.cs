namespace EF_pr8
{
    class Program
    {
        static void Main(string[] args)
        {
            // Демонстрация всех стратегий
            foreach (InheritanceStrategy strategy in Enum.GetValues(typeof(InheritanceStrategy)))
            {
                Console.WriteLine($"\n-- {strategy} Strategy --");

                using var context = ContextFactory.CreateContext(strategy);

                // Инициализация данных
                DataInitializer.Initialize(context);

                // Вывод результатов
                PrintStrategyInfo(context, strategy);
            }
        }

        // Вывод информации о стратегии
        static void PrintStrategyInfo(AirplaneContextBase context, InheritanceStrategy strategy)
        {
            Console.WriteLine($"Manufacturers created: {context.Manufacturers.Count()}");
            Console.WriteLine($"Total planes created: {context.Planes.Count()}");

            Console.WriteLine("\nPlane types distribution:");
            Console.WriteLine($"- Electrical: {context.ElectricalPlanes.Count()}");
            Console.WriteLine($"- Nuclear: {context.NuclearPlanes.Count()}");
            Console.WriteLine($"- Steam: {context.SteamPlanes.Count()}");

            Console.WriteLine("\nSample electrical planes:");
            context.ElectricalPlanes.Take(2).ToList().ForEach(p =>
                Console.WriteLine($"  {p.Model}, Battery: {p.BatteryCapacity}kWh"));
        }
    }
}