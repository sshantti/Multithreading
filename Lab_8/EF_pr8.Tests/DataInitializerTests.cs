using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EF_pr8.Tests
{
    public class DataInitializerTests
    {
        [Theory]
        [InlineData(InheritanceStrategy.TPH)]
        [InlineData(InheritanceStrategy.TPT)]
        [InlineData(InheritanceStrategy.TPC)]
        public void Initialize_ShouldCreateManufacturers(InheritanceStrategy strategy)
        {
            // Arrange
            var context = ContextFactory.CreateInMemoryContext(strategy);

            // Act
            DataInitializer.Initialize(context);

            // Assert
            Assert.Equal(DataInitializer.ManufacturerCount, context.Manufacturers.Count());
        }

        [Theory]
        [InlineData(InheritanceStrategy.TPH)]
        [InlineData(InheritanceStrategy.TPT)]
        [InlineData(InheritanceStrategy.TPC)]
        public void Initialize_ShouldCreatePlanes(InheritanceStrategy strategy)
        {
            // Arrange
            var context = ContextFactory.CreateInMemoryContext(strategy);

            // Act
            DataInitializer.Initialize(context);

            // Assert
            Assert.Equal(DataInitializer.PlaneCount, context.Planes.Count());
        }

        [Theory]
        [InlineData(InheritanceStrategy.TPH)]
        [InlineData(InheritanceStrategy.TPT)]
        [InlineData(InheritanceStrategy.TPC)]
        public void Initialize_ShouldCreateCorrectPlaneTypes(InheritanceStrategy strategy)
        {
            // Arrange
            var context = ContextFactory.CreateInMemoryContext(strategy);

            // Act
            DataInitializer.Initialize(context);

            // Assert
            var electricalCount = context.ElectricalPlanes.Count();
            var nuclearCount = context.NuclearPlanes.Count();
            var steamCount = context.SteamPlanes.Count();

            Assert.Equal(DataInitializer.PlaneCount, electricalCount + nuclearCount + steamCount);
        }
    }
}