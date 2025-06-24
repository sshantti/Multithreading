using Xunit;

namespace ADO.Net.Tests
{
    public class PlaneTests
    {
        [Fact]
        public void Create_ReturnsPlaneWithProperties()
        {
            // Act
            var plane = Plane.Create("SN001", "787", "B787", EngineType.Electrical);

            // Assert
            Assert.Equal("787", plane.Model);
            Assert.Equal("B787", plane.PlaneCode);
            Assert.Equal(EngineType.Electrical, plane.EngineType);
        }
    }
}