using ClassLibrary;
using System.Xml.Serialization;
using Xunit;

namespace Concurrency_pr5.Tests
{
    // Тестирование обработки файлов
    public class FileProcessorTests
    {
        // Проверка чтения данных из файла
        [Fact]
        public async Task ProcessFilesAsync_ReadsValidData()
        {
            // Arrange: Создание тестового файла
            var tempFile = Path.GetTempFileName();
            var progress = new ProgressBar(10);
            var plane = Plane.Create("SN123", "ModelX", "PC999", EngineType.Electrical);
            var objects = new List<object> { plane };

            var serializer = new XmlSerializer(typeof(List<object>),
                new[] { typeof(Plane), typeof(Manufacturer) });

            await using (var stream = new FileStream(tempFile, FileMode.Create))
            {
                serializer.Serialize(stream, objects);
            }

            var processor = new FileProcessor();

            // Act: Обработка файла
            await processor.ProcessFilesAsync(new[] { tempFile }, progress);

            // Assert: Проверка наличия данных
            Assert.Single(processor.Data[Path.GetFileName(tempFile)]);

            // Cleanup
            File.Delete(tempFile);
        }
    }
}