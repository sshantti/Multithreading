using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ClassLibrary;

namespace Concurrency_pr5
{
    public class FileProcessor
    {
        // Словарь: имя файла -> коллекция объектов
        public ConcurrentDictionary<string, ConcurrentBag<object>> Data { get; } = new();

        // Асинхронная обработка нескольких файлов
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

        // Обработка одного файла: чтение и добавление в словарь
        private async Task ProcessFileAsync(string filePath, IProgress<int>? progress)
        {
            try
            {
                var objects = await ReadFileAsync(filePath);
                Data[Path.GetFileName(filePath)] = new ConcurrentBag<object>(objects);

                // Сообщаем прогресс по каждому объекту
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