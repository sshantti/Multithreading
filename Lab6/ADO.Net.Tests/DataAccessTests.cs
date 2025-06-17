using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Data.SqlClient;

namespace ADO.Net.Tests
{
    public class DataAccessTests : DatabaseTestsBase, IAsyncLifetime
    {
        private readonly DataAccess _dataAccess = new DataAccess();

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await _dataAccess.AddManufacturerAsync("Initial", "Address", true);
        }

        public new Task DisposeAsync() => base.DisposeAsync();

        [Fact]
        public async Task AddManufacturerAsync_InsertsRecordSuccessfully()
        {
            // Act
            await _dataAccess.AddManufacturerAsync("TestMan", "TestAddress", true);

            // Assert
            using var connection = new SqlConnection(Constants.ConnectionString);
            await connection.OpenAsync();

            var cmd = new SqlCommand(
                $"SELECT COUNT(*) FROM {Constants.ManufacturersTable} " +
                $"WHERE {Constants.ManufacturerName} = 'TestMan'",
                connection);

            var count = (int)cmd.ExecuteScalar();
            Assert.Equal(1, count);
        }

        [Theory]
        [InlineData("SN123", "747", "BC1", EngineType.Steam)]
        [InlineData("SN456", "777", "BC2", EngineType.Nuclear)]
        public async Task AddPlaneAsync_InsertsDifferentModelsSuccessfully(
            string serial,
            string model,
            string code,
            EngineType engine)
        {
            // Arrange
            await _dataAccess.AddManufacturerAsync("Boeing", "Seattle", true);

            // Act
            await _dataAccess.AddPlaneAsync(serial, model, code, engine, 1);

            // Assert
            using var connection = new SqlConnection(Constants.ConnectionString);
            await connection.OpenAsync();

            var cmd = new SqlCommand(
                $"SELECT COUNT(*) FROM {Constants.PlanesTable} " +
                $"WHERE {Constants.PlaneSerialNumber} = '{serial}'",
                connection);

            var count = (int)cmd.ExecuteScalar();
            Assert.Equal(1, count);
        }
    }
}
