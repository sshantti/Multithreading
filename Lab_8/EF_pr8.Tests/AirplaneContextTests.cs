using Xunit;

namespace EF_pr8.Tests
{
    public class AirplaneContextTests
    {
        [Theory]
        [InlineData(InheritanceStrategy.TPH)]
        [InlineData(InheritanceStrategy.TPT)]
        [InlineData(InheritanceStrategy.TPC)]
        public void Initialize_ShouldCreateCorrectData(InheritanceStrategy strategy)
        {
            // Arrange
            var context = ContextFactory.CreateInMemoryContext(strategy);

            // Act
            DataInitializer.Initialize(context);

            // Assert
            Assert.Equal(DataInitializer.ManufacturerCount, context.Manufacturers.Count());
            Assert.Equal(DataInitializer.PlaneCount, context.Planes.Count());

            // Проверка распределения типов самолетов
            var total = context.ElectricalPlanes.Count() +
                        context.NuclearPlanes.Count() +
                        context.SteamPlanes.Count();

            Assert.Equal(DataInitializer.PlaneCount, total);
        }
    }
}