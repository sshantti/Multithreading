using System;
using System.Threading.Tasks;
using ClassLibrary;
using Microsoft.Data.SqlClient;

namespace ADO.Net_pr6
{
    class Program
    {
        const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=PlaneManufacturersDb;Integrated Security=True;";

        static async Task Main()
        {
            // Инициализация базы данных
            // - Создание БД, если не существует
            // - Открытие соединения с БД
            // - Создание таблиц Manufacturers и Planes

            DatabaseHelper.EnsureDatabaseExists(ConnectionString);
            await using var context = new DatabaseContext(ConnectionString);
            var initializer = new DatabaseInitializer(context);
            await initializer.InitializeAsync();

            // Подготовка репозиториев для работы с данным
            var manufacturerRepo = new ManufacturerRepository(context);
            var planeRepo = new PlaneRepository(context);
            var seeder = new DataSeeder(manufacturerRepo, planeRepo);

            // Основной цикл управления
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Aircraft Manufacturer Database");
                Console.WriteLine("1. Seed database (15 manufacturers + 30 planes)");
                Console.WriteLine("2. Add new manufacturer");
                Console.WriteLine("3. Add new plane");
                Console.WriteLine("4. View planes by manufacturer");
                Console.WriteLine("5. Exit");
                Console.Write("Select option: ");

                var option = Console.ReadLine();
                Console.Clear();

                try
                {
                    // Маршрутизация пользовательских команд

                    switch (option)
                    {
                        case "1":
                            await seeder.SeedAsync();
                            Console.WriteLine("\nDatabase seeded successfully!");
                            break;

                        case "2":
                            await AddManufacturer(manufacturerRepo);
                            break;

                        case "3":
                            await AddPlane(planeRepo);
                            break;

                        case "4":
                            await ViewPlanes(planeRepo);
                            break;

                        case "5":
                            Console.WriteLine("Exiting...");
                            return;

                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        // Добавление нового производителя самолетов
        // - Сбор информации о производителе
        // - Валидация ввода
        // - Сохранение в базу данных
        private static async Task AddManufacturer(ManufacturerRepository repo)
        {
            Console.WriteLine("Add New Manufacturer");
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Error: Manufacturer name cannot be empty!");
                return;
            }

            Console.Write("Address: ");
            var address = Console.ReadLine() ?? string.Empty;

            Console.Write("Is child company (true/false): ");
            if (!bool.TryParse(Console.ReadLine(), out bool isChild))
            {
                Console.WriteLine("Invalid boolean value! Using default: false");
                isChild = false;
            }

            var manufacturer = Manufacturer.Create(name, address, isChild);
            var id = await repo.AddAsync(manufacturer);

            Console.WriteLine($"Manufacturer added successfully! ID: {id}");
        }

        // Добавление нового самолета в каталог
        // - Сбор технических характеристик самолета
        // - Связь с производителем через ID
        // - Сохранение в базу данных
        private static async Task AddPlane(PlaneRepository planeRepo)
        {
            Console.WriteLine("Add New Plane");
            Console.Write("Manufacturer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int manId) || manId <= 0)
            {
                Console.WriteLine("Invalid manufacturer ID! Operation canceled.");
                return;
            }

            Console.Write("Serial Number: ");
            var serial = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(serial))
            {
                Console.WriteLine("Error: Serial number cannot be empty!");
                return;
            }

            Console.Write("Model: ");
            var model = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(model))
            {
                Console.WriteLine("Error: Model cannot be empty!");
                return;
            }

            Console.Write("Plane Code: ");
            var code = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Engine Types: 0-Electrical, 1-Nuclear, 2-Steam");
            Console.Write("Engine Type (0-2): ");
            EngineType engineType;
            if (!Enum.TryParse(Console.ReadLine(), out engineType) || !Enum.IsDefined(typeof(EngineType), engineType))
            {
                Console.WriteLine("Invalid engine type! Using default: Electrical");
                engineType = EngineType.Electrical;
            }

            var plane = Plane.Create(serial, model, code, engineType);
            var id = await planeRepo.AddAsync(plane, manId);

            Console.WriteLine($"Plane added successfully! ID: {id}");
        }

        // Просмотр каталога самолетов по производителю
        // - Поиск самолетов по ID производителя
        // - Отображение технических характеристик
        // - Форматированный вывод результатов
        private static async Task ViewPlanes(PlaneRepository repo)
        {
            Console.WriteLine("View Planes by Manufacturer");
            int manId;
            while (true)
            {
                Console.Write("Enter Manufacturer ID: ");
                if (int.TryParse(Console.ReadLine(), out manId))
                {
                    break;
                }
                Console.WriteLine("Invalid input. Please enter a valid integer ID.");
            }

            var planes = await repo.GetByManufacturerIdAsync(manId);

            Console.WriteLine($"\nPlanes for manufacturer ID {manId}:");
            foreach (var plane in planes)
            {
                Console.WriteLine($"- {plane.Model} ({plane.PlaneCode})");
                Console.WriteLine($"  Serial: {plane.SerialNumber}, Engine: {plane.EngineType}");
            }
            Console.WriteLine($"\nTotal planes: {planes.Count()}");
        }
    }
}