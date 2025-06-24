namespace EF_pr8
{
    // Автоматическое заполнение БД демонстрационными данными
    public static class DataInitializer
    {
        // Константы для количества создаваемых объектов
        public const int ManufacturerCount = 10;
        public const int PlaneCount = 30;

        // Инициализация БД тестовыми данными
        public static void Initialize(AirplaneContextBase context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var manufacturers = new List<Manufacturer>();
            var random = new Random();

            // Генерация производителей
            for (int i = 1; i <= ManufacturerCount; i++)
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

            // Генерация самолетов разных типов
            var planes = new List<Plane>();

            for (int i = 1; i <= PlaneCount; i++)
            {
                var manufacturer = manufacturers[random.Next(manufacturers.Count)];
                var planeType = i % 3; // 0, 1 или 2

                switch (planeType)
                {
                    case 0:
                        planes.Add(new ElectricalPlane
                        {
                            SerialNumber = $"ELEC_{i}",
                            Model = $"E-Model_{i}",
                            PlaneCode = $"EC_{i}",
                            BatteryCapacity = 100 + i * 10,
                            Voltage = 400 + i * 50,
                            ManufacturerId = manufacturer.Id
                        });
                        break;

                    case 1:
                        planes.Add(new NuclearPlane
                        {
                            SerialNumber = $"NUC_{i}",
                            Model = $"N-Model_{i}",
                            PlaneCode = $"NC_{i}",
                            ReactorType = $"RT-{i}",
                            FuelRodLife = 5 + i,
                            ManufacturerId = manufacturer.Id
                        });
                        break;

                    case 2:
                        planes.Add(new SteamPlane
                        {
                            SerialNumber = $"STEAM_{i}",
                            Model = $"S-Model_{i}",
                            PlaneCode = $"SC_{i}",
                            BoilerPressure = 1.5 + i * 0.1,
                            SteamTemperature = 150 + i * 10,
                            ManufacturerId = manufacturer.Id
                        });
                        break;
                }
            }

            context.Planes.AddRange(planes);
            context.SaveChanges();
        }
    }
}