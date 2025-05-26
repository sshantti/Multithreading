using ClassLibrary;
using System.Xml.Serialization;

namespace Concurrency_pr5
{
    public static class Generator
    {
        private const int TotalFiles = 5;
        private const int ObjectsPerFile = 10;

        // Генерация нескольких файлов
        public static async Task GenerateFilesAsync(string outputDirectory, IProgress<int>? progress = null)
        {
            try
            {
                Directory.CreateDirectory(outputDirectory);
                var tasks = new List<Task>();
                var rnd = new Random();

                for (int i = 0; i < TotalFiles; i++)
                {
                    var filePath = Path.Combine(outputDirectory, $"data_{i}.xml");
                    tasks.Add(GenerateFileAsync(
                        filePath, 
                        ObjectsPerFile, 
                        rnd, 
                        progress));
                }

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating files: {ex.Message}");
                throw;
            }
        }

        // Генерация одного файла
        internal static async Task GenerateFileAsync(string filePath, int count, Random rnd, IProgress<int>? progress)
        {
            try
            {
                var objects = new List<object>();
                for (int j = 0; j < count; j++)
                {
                    if (j % 2 == 0)
                    {
                        objects.Add(Plane.Create(
                            $"SN-{rnd.Next(1000, 9999)}",
                            $"Model-{rnd.Next(100, 999)}",
                            $"PC-{rnd.Next(1000, 9999)}",
                            (EngineType)rnd.Next(0, 3)
                        ));
                    }
                    else
                    {
                        objects.Add(Manufacturer.Create(
                            $"Manuf-{rnd.Next(100, 999)}",
                            $"Address-{rnd.Next(1, 100)} St.",
                            rnd.Next(2) == 0
                        ));
                    }
                    progress?.Report(1);
                }

                await SerializeObjectsAsync(objects, filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in file {filePath}: {ex.Message}");
                throw;
            }
        }

        // Асинхронная сериализация объектов в XML-файл
        private static async Task SerializeObjectsAsync(List<object> objects, string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<object>), new[] { typeof(Plane), typeof(Manufacturer) });

                await using var stream = new FileStream(filePath, FileMode.Create);
                await Task.Run(() => serializer.Serialize(stream, objects));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing data: {ex.Message}");
                throw;
            }
        }
    }
}