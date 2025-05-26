using ClassLibrary;
using System.Collections.Concurrent;

namespace Concurrency_pr5
{
    // Координация всех этапов обработки данных.
    class Program
    {
        static async Task Main(string[] args)
        {
            const string dataDir = "Data";
            var processor = new FileProcessor();

            try
            {
                Console.WriteLine("Generating files...");
                await Generator.GenerateFilesAsync(dataDir, new ProgressBar(50));

                Console.WriteLine("\n\nProcessing files...");
                var files = Directory.GetFiles(dataDir, "*.xml");
                await processor.ProcessFilesAsync(files, new ProgressBar(50));

                Console.WriteLine("\n\nSorting data...");
                DataProcessor.SortData(processor.Data);

                Console.WriteLine("\nResults:");
                PrintData(processor.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nCritical error: {ex.Message}");
            }
        }

        // Вывод содержимого словаря
        private static void PrintData(ConcurrentDictionary<string, ConcurrentBag<object>> data)
        {
            foreach (var (fileName, objects) in data)
            {
                Console.WriteLine($"\nFile: {fileName}");
                foreach (var obj in objects)
                {
                    if (obj is Plane plane)
                        plane.PrintObject();
                    else if (obj is Manufacturer manufacturer)
                        manufacturer.PrintObject();
                }
            }
        }
    }
}