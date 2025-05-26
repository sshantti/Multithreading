using Xunit;
using System.Collections.Concurrent;
using ClassLibrary;

namespace Concurrency_pr5.Tests
{
    // Тестирование функциональности сортировки данных.
    public class DataProcessorTests
    {
        // Проверка сортировки смешанных данных
        [Fact]
        public void SortData_SortsCorrectly()
        {
            // Arrange: Подготовка тестовых данных
            var data = new ConcurrentDictionary<string, ConcurrentBag<object>>();
            var objects = new ConcurrentBag<object>
            {
                Plane.Create("SN2", "M2", "PC2", EngineType.Electrical),
                Manufacturer.Create("BCompany", "Address2", false)
            };
            data.TryAdd("test", objects);

            var processor = new DataProcessor();

            // Act: Вызов метода сортировки
            DataProcessor.SortData(data);

            // Assert: Проверка, что первым элементом стал самолет
            var firstItem = data["test"].First();
            Assert.IsType<Plane>(firstItem);
        }
    }
}