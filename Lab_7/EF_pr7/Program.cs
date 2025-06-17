using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using ClassLibrary;

namespace EF_pr7
{
    class Program
    {
        static void Main(string[] args)
        {
            // Настройка DI контейнера
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PlaneDB;Trusted_Connection=True;TrustServerCertificate=True;"));

            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<BusinessService>();

            var provider = services.BuildServiceProvider();

            // Инициализация базы
            using (var scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DataInitializer.Initialize(context);
            }

            // Главное меню
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. CRUD for Manufacturers");
                Console.WriteLine("2. CRUD for Planes");
                Console.WriteLine("3. Add new Plane for new Manufacturer");
                Console.WriteLine("4. Get Planes by Manufacturer");
                Console.WriteLine("5. Exit");

                switch (Console.ReadLine())
                {
                    case "1": ShowManufacturerMenu(provider); break;
                    case "2": ShowPlaneMenu(provider); break;
                    case "3": AddNewPlaneForNewManufacturer(provider); break;
                    case "4": GetPlanesByManufacturer(provider); break;
                    case "5": return;
                }
            }
        }

        static void ShowManufacturerMenu(IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IRepository<Manufacturer>>();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Manufacturers:\n1.List\n2.Add\n3.Update\n4.Delete\n5.Back");
                switch (Console.ReadLine())
                {
                    case "1":
                        foreach (var m in repo.GetAll())
                            Console.WriteLine($"{m.Id}: {m.Name}");
                        break;
                    case "2":
                        repo.Add(new Manufacturer
                        {
                            Name = Read("Name"),
                            Address = Read("Address"),
                            IsAChildCompany = bool.Parse(Read("Is Child Company (true/false)"))
                        });
                        repo.Save();
                        break;
                    case "3":
                        Console.Write("Enter ID to update: ");
                        int id = int.Parse(Console.ReadLine());
                        var entity = repo.GetById(id);
                        if (entity != null)
                        {
                            entity.Name = Read($"Name ({entity.Name}): ") ?? entity.Name;
                            entity.Address = Read($"Address ({entity.Address}): ") ?? entity.Address;
                            entity.IsAChildCompany = bool.Parse(Read($"Is Child ({entity.IsAChildCompany}): "));
                            repo.Update(entity);
                            repo.Save();
                        }
                        break;
                    case "4":
                        Console.Write("Enter ID to delete: ");
                        repo.Delete(int.Parse(Console.ReadLine()));
                        repo.Save();
                        break;
                    case "5": return;
                }
                Console.ReadKey();
            }
        }

        static void ShowPlaneMenu(IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IRepository<Plane>>();
            var manRepo = scope.ServiceProvider.GetRequiredService<IRepository<Manufacturer>>();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Planes:\n1.List\n2.Add\n3.Update\n4.Delete\n5.Back");
                switch (Console.ReadLine())
                {
                    case "1":
                        foreach (var p in repo.GetAll())
                            Console.WriteLine($"{p.Id}: {p.Model} [{p.SerialNumber}]");
                        break;
                    case "2":
                        var plane = new Plane
                        {
                            SerialNumber = Read("Serial Number"),
                            Model = Read("Model"),
                            PlaneCode = Read("Plane Code"),
                            EngineType = Enum.Parse<EngineType>(Read("Engine Type (0-Electrical,1-Nuclear,2-Steam)")),
                            ManufacturerId = int.Parse(Read("Manufacturer ID"))
                        };

                        if (manRepo.GetById(plane.ManufacturerId) != null)
                        {
                            repo.Add(plane);
                            repo.Save();
                        }
                        else
                        {
                            Console.WriteLine("Manufacturer not found!");
                        }
                        break;
                    case "3":
                        Console.Write("Enter ID to update: ");
                        int id = int.Parse(Console.ReadLine());
                        var entity = repo.GetById(id);
                        if (entity != null)
                        {
                            entity.SerialNumber = Read($"Serial ({entity.SerialNumber}): ") ?? entity.SerialNumber;
                            entity.Model = Read($"Model ({entity.Model}): ") ?? entity.Model;
                            entity.PlaneCode = Read($"Code ({entity.PlaneCode}): ") ?? entity.PlaneCode;
                            entity.EngineType = Enum.Parse<EngineType>(Read($"Engine ({entity.EngineType}): "));
                            entity.ManufacturerId = int.Parse(Read($"Manufacturer ID ({entity.ManufacturerId}): "));
                            repo.Update(entity);
                            repo.Save();
                        }
                        break;
                    case "4":
                        Console.Write("Enter ID to delete: ");
                        repo.Delete(int.Parse(Console.ReadLine()));
                        repo.Save();
                        break;
                    case "5": return;
                }
                Console.ReadKey();
            }
        }

        static void AddNewPlaneForNewManufacturer(IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<BusinessService>();

            var manufacturer = new Manufacturer
            {
                Name = Read("Manufacturer Name"),
                Address = Read("Address"),
                IsAChildCompany = bool.Parse(Read("Is Child Company (true/false)"))
            };

            var plane = new Plane
            {
                SerialNumber = Read("Serial Number"),
                Model = Read("Model"),
                PlaneCode = Read("Plane Code"),
                EngineType = Enum.Parse<EngineType>(Read("Engine Type (0-Electrical,1-Nuclear,2-Steam)"))
            };

            Console.WriteLine(service.AddNewPlaneForNewManufacturer(manufacturer, plane)
                ? "Success!" : "Transaction failed!");
            Console.ReadKey();
        }

        static void GetPlanesByManufacturer(IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var manufacturerId = int.Parse(Read("Manufacturer ID"));
            var planes = context.Planes
                .Where(p => p.ManufacturerId == manufacturerId)
                .ToList();

            Console.WriteLine($"Planes for manufacturer {manufacturerId}:");
            foreach (var p in planes)
                Console.WriteLine($"  {p.Model} - {p.SerialNumber}");
            Console.ReadKey();
        }

        static string Read(string prompt)
        {
            Console.Write($"{prompt}: ");
            return Console.ReadLine()!;
        }
    }
}