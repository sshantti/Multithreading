using Microsoft.EntityFrameworkCore;

namespace EF_pr7
{
    public static class DataInitializer
    {
        // Инициализатор базы данных с тестовыми данными
        public static void Initialize(AirplaneContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var manufacturers = new List<Manufacturer>();
            var planes = new List<Plane>();
            var random = new Random();

            // Генерация 30 производителей
            for (int i = 1; i <= 30; i++)
            {
                manufacturers.Add(new Manufacturer
                {
                    Name = $"Manufacturer_{i}",
                    Address = $"Address_{i}",
                    IsAChildCompany = i % 3 == 0
                });
            }
            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            // Генерация 30 самолётов
            for (int i = 1; i <= 30; i++)
            {
                planes.Add(new Plane
                {
                    SerialNumber = $"SN_{i}",
                    Model = $"Model_{i % 10}",
                    PlaneCode = $"PC_{i}",
                    EngineType = (EngineType)(i % 3),
                    ManufacturerId = manufacturers[random.Next(manufacturers.Count)].Id
                });
            }
            context.Planes.AddRange(planes);
            context.SaveChanges();
        }
    }
}