using Xunit;
using System.Collections.Concurrent;
using ClassLibrary;

namespace Concurrency_pr5.Tests
{
    public class DataProcessorTests
    {
        [Fact]
        public void SortData_SortsCorrectly()
        {
            // Arrange
            var data = new ConcurrentDictionary<string, ConcurrentBag<object>>();
            var objects = new ConcurrentBag<object>
            {
                Plane.Create("SN2", "M2", "PC2", EngineType.Electrical),
                Manufacturer.Create("BCompany", "Address2", false)
            };
            data.TryAdd("test", objects);

            var processor = new DataProcessor();

            // Act
            DataProcessor.SortData(data);

            // Assert через публичные методы
            var firstItem = data["test"].First();
            Assert.IsType<Plane>(firstItem);
        }
    }
}