using Xunit;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClassLibrary;
using System.Xml.Serialization;

namespace Concurrency_pr5.Tests
{
    public class FileProcessorTests
    {
        [Fact]
        public async Task ProcessFilesAsync_ReadsValidData()
        {
            // Arrange
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

            // Act
            await processor.ProcessFilesAsync(new[] { tempFile }, progress);

            // Assert
            Assert.Single(processor.Data[Path.GetFileName(tempFile)]);

            // Cleanup
            File.Delete(tempFile);
        }
    }
}