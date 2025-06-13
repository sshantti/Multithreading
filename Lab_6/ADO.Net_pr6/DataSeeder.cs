using System;
using System.Threading.Tasks;
using ClassLibrary;

namespace ADO.Net_pr6
{
    // Первоначальное заполнение базы данных
    // - Генерация тестовых данных для производителей
    // - Генерация тестовых данных для самолетов
    // - Создание связей между производителями и самолетами
    public class DataSeeder
    {
        private readonly ManufacturerRepository _manufacturerRepo;
        private readonly PlaneRepository _planeRepo;

        public DataSeeder(
            ManufacturerRepository manufacturerRepo,
            PlaneRepository planeRepo)
        {
            _manufacturerRepo = manufacturerRepo;
            _planeRepo = planeRepo;
        }

        public async Task SeedAsync()
        {
            var random = new Random();
            for (int i = 1; i <= Constants.ManufacturerCount; i++)
            {
                var manufacturer = Manufacturer.Create(
                    $"Manufacturer_{i}",
                    $"Address_{i}",
                    i % 2 == 0
                );

                var manufacturerId = await _manufacturerRepo.AddAsync(manufacturer);

                for (int j = 1; j <= Constants.PlanesPerManufacturer; j++)
                {
                    var plane = Plane.Create(
                        $"SN_{i}_{j}",
                        $"Model_{i}_{j}",
                        $"PC_{i}_{j}",
                        (EngineType)random.Next(0, 3)
                    );

                    await _planeRepo.AddAsync(plane, manufacturerId);
                }
            }
        }
    }
}
