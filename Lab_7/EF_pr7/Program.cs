using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EF_pr7
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Настройка DI-контейнера
            var serviceProvider = new ServiceCollection()
                .AddDbContext<AirplaneContext>(options =>
                    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AirplaneDb;Trusted_Connection=True;"))
                .AddScoped<Repository<Manufacturer>>()
                .AddScoped<Repository<Plane>>()
                .AddScoped<BusinessService>()
                .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AirplaneContext>();
                // Инициализация БД с тестовыми данными
                DataInitializer.Initialize(context);
                // Получение сервисов
                var manufacturerRepo = scope.ServiceProvider.GetRequiredService<Repository<Manufacturer>>();
                var planeRepo = scope.ServiceProvider.GetRequiredService<Repository<Plane>>();
                var businessService = scope.ServiceProvider.GetRequiredService<BusinessService>();

                while (true)
                {
                    // Главное меню
                    Console.Clear();
                    Console.WriteLine("1. CRUD for Manufacturers");
                    Console.WriteLine("2. CRUD for Planes");
                    Console.WriteLine("3. Add Plane with New Manufacturer");
                    Console.WriteLine("4. Get Planes by Manufacturer ID");
                    Console.WriteLine("5. Exit");
                    Console.Write("Select an option: ");
                    // Обработка выбора пользователя    
                    var choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            await CrudManufacturerMenu(manufacturerRepo);
                            break;
                        case "2":
                            await CrudPlaneMenu(planeRepo);
                            break;
                        case "3":
                            await AddPlaneWithManufacturerMenu(businessService);
                            break;
                        case "4":
                            await GetPlanesByManufacturerMenu(businessService);
                            break;
                        case "5":
                            return;
                    }
                }
            }
        }

        static async Task CrudManufacturerMenu(Repository<Manufacturer> repo)
        {
            Console.Clear();
            Console.WriteLine("Manufacturer CRUD Operations");
            Console.WriteLine("1. Add Manufacturer");
            Console.WriteLine("2. Get Manufacturer by ID");
            Console.WriteLine("3. List All Manufacturers");
            Console.WriteLine("4. Update Manufacturer");
            Console.WriteLine("5. Delete Manufacturer");
            Console.Write("Select an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Write("Name: ");
                    var name = Console.ReadLine() ?? string.Empty;
                    Console.Write("Address: ");
                    var address = Console.ReadLine() ?? string.Empty;

                    bool isChild = false;
                    Console.Write("Is child company (true/false): ");
                    if (bool.TryParse(Console.ReadLine(), out bool parsedBool))
                    {
                        isChild = parsedBool;
                    }

                    await repo.AddAsync(new Manufacturer
                    {
                        Name = name,
                        Address = address,
                        IsAChildCompany = isChild
                    });
                    await repo.SaveAsync();
                    break;

                case "2":
                    Console.Write("Enter ID: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var manufacturer = await repo.GetAsync(id);
                        if (manufacturer != null)
                        {
                            Console.WriteLine($"ID: {manufacturer.Id}, Name: {manufacturer.Name}");
                        }
                        else
                        {
                            Console.WriteLine("Not found");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID format");
                    }
                    Console.ReadKey();
                    break;

                case "3":
                    var manufacturers = await repo.GetAllAsync();
                    foreach (var m in manufacturers)
                    {
                        Console.WriteLine($"ID: {m.Id}, Name: {m.Name}");
                    }
                    Console.ReadKey();
                    break;

                case "4":
                    Console.Write("Enter ID to update: ");
                    if (int.TryParse(Console.ReadLine(), out int updateId))
                    {
                        var toUpdate = await repo.GetAsync(updateId);
                        if (toUpdate != null)
                        {
                            Console.Write("New Name: ");
                            var newName = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newName))
                            {
                                toUpdate.Name = newName;
                            }
                            repo.Update(toUpdate);
                            await repo.SaveAsync();
                        }
                        else
                        {
                            Console.WriteLine("Not found");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID format");
                    }
                    break;

                case "5":
                    Console.Write("Enter ID to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        var toDelete = await repo.GetAsync(deleteId);
                        if (toDelete != null)
                        {
                            repo.Delete(toDelete);
                            await repo.SaveAsync();
                        }
                        else
                        {
                            Console.WriteLine("Not found");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID format");
                    }
                    break;
            }
        }

        static async Task CrudPlaneMenu(Repository<Plane> repo)
        {
            Console.Clear();
            Console.WriteLine("Plane CRUD Operations");
            Console.WriteLine("1. Add Plane");
            Console.WriteLine("2. Get Plane by ID");
            Console.WriteLine("3. List All Planes");
            Console.WriteLine("4. Update Plane");
            Console.WriteLine("5. Delete Plane");
            Console.Write("Select an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Write("Serial Number: ");
                    var sn = Console.ReadLine() ?? string.Empty;
                    Console.Write("Model: ");
                    var model = Console.ReadLine() ?? string.Empty;
                    Console.Write("Plane Code: ");
                    var code = Console.ReadLine() ?? string.Empty;

                    EngineType engineType = EngineType.Electrical;
                    Console.Write("Engine Type (0-Electrical, 1-Nuclear, 2-Steam): ");
                    if (Enum.TryParse(Console.ReadLine(), out EngineType parsedEngine) &&
                        Enum.IsDefined(typeof(EngineType), parsedEngine))
                    {
                        engineType = parsedEngine;
                    }

                    Console.Write("Manufacturer ID: ");
                    if (int.TryParse(Console.ReadLine(), out int manufacturerId))
                    {
                        await repo.AddAsync(new Plane
                        {
                            SerialNumber = sn,
                            Model = model,
                            PlaneCode = code,
                            EngineType = engineType,
                            ManufacturerId = manufacturerId
                        });
                        await repo.SaveAsync();
                    }
                    else
                    {
                        Console.WriteLine("Invalid Manufacturer ID");
                    }
                    break;

                case "2":
                    Console.Write("Enter ID: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var plane = await repo.GetAsync(id);
                        if (plane != null)
                        {
                            Console.WriteLine($"ID: {plane.Id}, Model: {plane.Model}");
                        }
                        else
                        {
                            Console.WriteLine("Not found");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID format");
                    }
                    Console.ReadKey();
                    break;

                case "3":
                    var planes = await repo.GetAllAsync();
                    foreach (var p in planes)
                    {
                        Console.WriteLine($"ID: {p.Id}, Model: {p.Model}");
                    }
                    Console.ReadKey();
                    break;

                case "4":
                    Console.Write("Enter ID to update: ");
                    if (int.TryParse(Console.ReadLine(), out int updateId))
                    {
                        var toUpdate = await repo.GetAsync(updateId);
                        if (toUpdate != null)
                        {
                            Console.Write("New Model: ");
                            var newModel = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newModel))
                            {
                                toUpdate.Model = newModel;
                            }
                            repo.Update(toUpdate);
                            await repo.SaveAsync();
                        }
                        else
                        {
                            Console.WriteLine("Not found");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID format");
                    }
                    break;

                case "5":
                    Console.Write("Enter ID to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        var toDelete = await repo.GetAsync(deleteId);
                        if (toDelete != null)
                        {
                            repo.Delete(toDelete);
                            await repo.SaveAsync();
                        }
                        else
                        {
                            Console.WriteLine("Not found");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID format");
                    }
                    break;
            }
        }
        // Добавление самолета с новым производителем
        static async Task AddPlaneWithManufacturerMenu(BusinessService service)
        {
            Console.Clear();
            Console.WriteLine("Add New Plane with New Manufacturer");

            Console.Write("Manufacturer Name: ");
            var name = Console.ReadLine() ?? string.Empty;
            Console.Write("Manufacturer Address: ");
            var address = Console.ReadLine() ?? string.Empty;

            bool isChild = false;
            Console.Write("Is child company (true/false): ");
            if (bool.TryParse(Console.ReadLine(), out bool parsedBool))
            {
                isChild = parsedBool;
            }

            Console.Write("Plane Serial Number: ");
            var sn = Console.ReadLine() ?? string.Empty;
            Console.Write("Plane Model: ");
            var model = Console.ReadLine() ?? string.Empty;
            Console.Write("Plane Code: ");
            var code = Console.ReadLine() ?? string.Empty;

            EngineType engineType = EngineType.Electrical;
            Console.Write("Engine Type (0-Electrical, 1-Nuclear, 2-Steam): ");
            if (Enum.TryParse(Console.ReadLine(), out EngineType parsedEngine) &&
                Enum.IsDefined(typeof(EngineType), parsedEngine))
            {
                engineType = parsedEngine;
            }

            await service.AddPlaneWithManufacturerAsync(
                new Plane
                {
                    SerialNumber = sn,
                    Model = model,
                    PlaneCode = code,
                    EngineType = engineType
                },
                new Manufacturer
                {
                    Name = name,
                    Address = address,
                    IsAChildCompany = isChild
                }
            );
        }
        // Получение самолетов производителя
        static async Task GetPlanesByManufacturerMenu(BusinessService service)
        {
            Console.Clear();
            Console.Write("Enter Manufacturer ID: ");
            if (int.TryParse(Console.ReadLine(), out int manufacturerId))
            {
                var planes = await service.GetPlanesByManufacturerIdAsync(manufacturerId);

                if (planes.Any())
                {
                    Console.WriteLine($"Planes for manufacturer ID {manufacturerId}:");
                    foreach (var plane in planes)
                    {
                        Console.WriteLine($"- {plane.Model} ({plane.PlaneCode})");
                    }
                }
                else
                {
                    Console.WriteLine("No planes found for this manufacturer");
                }
            }
            else
            {
                Console.WriteLine("Invalid Manufacturer ID format");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}