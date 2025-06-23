using Xunit;

namespace ADO.Net.Tests
{
    public class ManufacturerTests
    {
        [Fact]
        public void Create_ReturnsManufacturerWithProperties()
        {
            // Act
            var manufacturer = Manufacturer.Create("Boeing", "Seattle", true);

            // Assert
            Assert.Equal("Boeing", manufacturer.Name);
            Assert.Equal("Seattle", manufacturer.Address);
            Assert.True(manufacturer.IsAChildCompany);
        }
    }
}