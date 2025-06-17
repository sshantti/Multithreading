using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ADO.Net
{
    class Program
    {
        private static readonly DatabaseManager dbManager = new DatabaseManager();
        private static readonly DataInserter dataInserter = new DataInserter();
        private static readonly DataAccess dataAccess = new DataAccess();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Aircraft Database Management System");
            Console.WriteLine("Initializing database...");
            dbManager.InitializeDatabase();

            while (true)
            {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1. Populate with sample data (30 records each)");
                Console.WriteLine("2. Add new manufacturer");
                Console.WriteLine("3. Add new plane");
                Console.WriteLine("4. Get planes by manufacturer");
                Console.WriteLine("5. List all manufacturers");
                Console.WriteLine("6. Exit");
                Console.Write("Select option: ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await PopulateSampleDataAsync();
                            break;
                        case "2":
                            await AddManufacturerAsync();
                            break;
                        case "3":
                            await AddPlaneAsync();
                            break;
                        case "4":
                            await GetPlanesByManufacturerAsync();
                            break;
                        case "5":
                            await ListManufacturersAsync();
                            break;
                        case "6":
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
            }
        }

        private static async Task PopulateSampleDataAsync()
        {
            Console.WriteLine("Adding 30 manufacturers and planes...");
            await dataInserter.InsertDataAsync();
            Console.WriteLine("Data added successfully!");
        }

        private static async Task AddManufacturerAsync()
        {
            Console.Write("Enter manufacturer name: ");
            var name = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter address: ");
            var address = Console.ReadLine() ?? string.Empty;

            Console.Write("Is child company? (y/n): ");
            var isChild = Console.ReadLine()?.ToLower() == "y";

            await dataAccess.AddManufacturerAsync(name, address, isChild);
            Console.WriteLine("Manufacturer added successfully!");
        }

        private static async Task AddPlaneAsync()
        {
            var manufacturers = await dataAccess.GetAllManufacturersAsync();
            Console.WriteLine("\nAvailable Manufacturers:");
            foreach (var m in manufacturers)
            {
                Console.WriteLine($"{m.Id}: {m.Name}");
            }

            Console.Write("\nEnter manufacturer ID: ");
            if (!int.TryParse(Console.ReadLine(), out var manufacturerId))
            {
                Console.WriteLine("Invalid manufacturer ID!");
                return;
            }

            Console.Write("Enter serial number: ");
            var serial = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter model: ");
            var model = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter plane code: ");
            var code = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Engine types: 0=Electrical, 1=Nuclear, 2=Steam");
            Console.Write("Select engine type (0-2): ");
            if (!Enum.TryParse(Console.ReadLine(), out EngineType engineType))
            {
                Console.WriteLine("Invalid engine type!");
                return;
            }

            await dataAccess.AddPlaneAsync(serial, model, code, engineType, manufacturerId);
            Console.WriteLine("Plane added successfully!");
        }

        private static async Task GetPlanesByManufacturerAsync()
        {
            Console.Write("Enter manufacturer name: ");
            var name = Console.ReadLine();

            var planes = await dataAccess.GetPlanesByManufacturerAsync(name);

            Console.WriteLine($"\nPlanes for manufacturer '{name}':");
            foreach (var plane in planes)
            {
                Console.WriteLine($"- {plane.Model} ({plane.PlaneCode}), Engine: {plane.EngineType}");
            }
        }

        private static async Task ListManufacturersAsync()
        {
            var manufacturers = await dataAccess.GetAllManufacturersAsync();

            Console.WriteLine("\nManufacturers:");
            foreach (var m in manufacturers)
            {
                Console.WriteLine($"ID: {m.Id}, Name: {m.Name}, Address: {m.Address}");
            }
        }
    }
}