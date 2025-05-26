using ClassLibrary;
using System.Collections.Concurrent;
using System.Xml.Serialization;

namespace Concurrency_pr5
{
    // Чтение и обработка данных из XML-файлов.
    public class FileProcessor
    {
        public ConcurrentDictionary<string, ConcurrentBag<object>> Data { get; } = new();

        // Асинхронная обработка нескольких файлов.
        public async Task ProcessFilesAsync(string[] filePaths, IProgress<int> progress)
        {
            try
            {
                var tasks = filePaths.Select(path => ProcessFileAsync(path, progress));
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing files: {ex.Message}");
                throw;
            }
        }

        // Обработка одного файла: чтение и добавление в словарь.
        private async Task ProcessFileAsync(string filePath, IProgress<int>? progress)
        {
            try
            {
                var objects = await ReadFileAsync(filePath);
                Data[Path.GetFileName(filePath)] = new ConcurrentBag<object>(objects);

                foreach (var _ in objects)
                {
                    progress?.Report(1);
                    await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
                throw;
            }
        }

        // Асинхронное чтение и десериализация XML
        private static async Task<List<object>> ReadFileAsync(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<object>), new[] { typeof(Plane), typeof(Manufacturer) });

                await using var stream = new FileStream(filePath, FileMode.Open);
                var result = serializer.Deserialize(stream) as List<object>;

                return result ?? throw new InvalidOperationException("Deserialized data is null");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing file: {ex.Message}");
                throw;
            }
        }
    }
}