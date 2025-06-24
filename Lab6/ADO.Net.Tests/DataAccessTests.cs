using Moq;
using System.Data.SqlClient;
using System.Data;
using Xunit;

namespace ADO.Net.Tests
{
    public class DataAccessTests
    {
        private readonly Mock<SqlConnection> _mockConnection;
        private readonly DataAccess _dataAccess;

        public DataAccessTests()
        {
            _mockConnection = new Mock<SqlConnection>();
            _dataAccess = new DataAccess
            {
                CreateConnection = () => _mockConnection.Object,
                CreateCommand = (conn, sql) => new SqlCommand(sql, conn)
            };
        }

        [Fact]
        public async Task AddManufacturerAsync_ExecutesCommandWithParameters()
        {
            // Arrange
            var mockCommand = new Mock<SqlCommand>();
            mockCommand.Setup(c => c.ExecuteScalarAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<object>(1));

            _mockConnection.Setup(c => c.OpenAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mockConnection.Setup(c => c.CreateCommand()).Returns(mockCommand.Object);

            // Act
            await _dataAccess.AddManufacturerAsync("Boeing", "Seattle", true);

            // Assert
            mockCommand.VerifySet(c => c.CommandText = Constants.InsertManufacturer, Times.Once);
            mockCommand.Verify(c => c.Parameters.AddWithValue("@name", "Boeing"), Times.Once);
            mockCommand.Verify(c => c.Parameters.AddWithValue("@address", "Seattle"), Times.Once);
            mockCommand.Verify(c => c.Parameters.AddWithValue("@isChild", true), Times.Once);
            mockCommand.Verify(c => c.ExecuteScalarAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddPlaneAsync_ExecutesCommandWithParameters()
        {
            // Arrange
            var mockCommand = new Mock<SqlCommand>();
            mockCommand.Setup(c => c.ExecuteNonQueryAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _mockConnection.Setup(c => c.OpenAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mockConnection.Setup(c => c.CreateCommand()).Returns(mockCommand.Object);

            // Act
            await _dataAccess.AddPlaneAsync("SN123", "787", "B787", EngineType.Electrical, 1);

            // Assert
            mockCommand.VerifySet(c => c.CommandText = Constants.InsertPlane, Times.Once);
            mockCommand.Verify(c => c.Parameters.AddWithValue("@serial", "SN123"), Times.Once);
            mockCommand.Verify(c => c.Parameters.AddWithValue("@model", "787"), Times.Once);
            mockCommand.Verify(c => c.Parameters.AddWithValue("@code", "B787"), Times.Once);
            mockCommand.Verify(c => c.Parameters.AddWithValue("@engine", (int)EngineType.Electrical), Times.Once);
            mockCommand.Verify(c => c.Parameters.AddWithValue("@manufacturerId", 1), Times.Once);
            mockCommand.Verify(c => c.ExecuteNonQueryAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetPlanesByManufacturerAsync_ReturnsPlanes()
        {
            // Arrange
            var mockReader = new Mock<SqlDataReader>();
            mockReader.SetupSequence(r => r.ReadAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);
            mockReader.Setup(r => r.GetString(0)).Returns("SN001");
            mockReader.Setup(r => r.GetString(1)).Returns("ModelX");
            mockReader.Setup(r => r.GetString(2)).Returns("CodeX");
            mockReader.Setup(r => r.GetInt32(3)).Returns(0);

            var mockCommand = new Mock<SqlCommand>();
            mockCommand.Setup(c => c.ExecuteReaderAsync(It.IsAny<CommandBehavior>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockReader.Object);

            _mockConnection.Setup(c => c.OpenAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mockConnection.Setup(c => c.CreateCommand()).Returns(mockCommand.Object);

            // Act
            var result = await _dataAccess.GetPlanesByManufacturerAsync("Boeing");

            // Assert
            var plane = result.Single();
            Assert.Equal("ModelX", plane.Model);
            Assert.Equal("CodeX", plane.PlaneCode);
            Assert.Equal(EngineType.Electrical, plane.EngineType);
        }

        [Fact]
        public async Task GetAllManufacturersAsync_ReturnsManufacturers()
        {
            // Arrange
            var mockReader = new Mock<SqlDataReader>();
            mockReader.SetupSequence(r => r.ReadAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);
            mockReader.Setup(r => r.GetInt32(0)).Returns(1);
            mockReader.Setup(r => r.GetString(1)).Returns("Boeing");
            mockReader.Setup(r => r.GetString(2)).Returns("Seattle");

            var mockCommand = new Mock<SqlCommand>();
            mockCommand.Setup(c => c.ExecuteReaderAsync(It.IsAny<CommandBehavior>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockReader.Object);

            _mockConnection.Setup(c => c.OpenAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mockConnection.Setup(c => c.CreateCommand()).Returns(mockCommand.Object);

            // Act
            var result = await _dataAccess.GetAllManufacturersAsync();

            // Assert
            var manufacturer = result.Single();
            Assert.Equal(1, manufacturer.Id);
            Assert.Equal("Boeing", manufacturer.Name);
            Assert.Equal("Seattle", manufacturer.Address);
        }
    }
}