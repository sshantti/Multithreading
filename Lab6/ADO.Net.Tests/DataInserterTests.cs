using Moq;
using System.Data.SqlClient;
using Xunit;

namespace ADO.Net.Tests
{
    public class DataInserterTests
    {
        [Fact]
        public async Task InsertDataAsync_Inserts30RecordsEach()
        {
            // Arrange
            var mockConnection = new Mock<SqlConnection>();
            var mockCommand = new Mock<SqlCommand>();

            mockCommand.Setup(c => c.ExecuteScalarAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<object>(1));

            mockCommand.Setup(c => c.ExecuteNonQueryAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            mockConnection.Setup(c => c.OpenAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            mockConnection.Setup(c => c.CreateCommand()).Returns(mockCommand.Object);

            var inserter = new DataInserter
            {
                CreateConnection = () => mockConnection.Object,
                CreateCommand = (conn, sql) => mockCommand.Object
            };

            // Act
            await inserter.InsertDataAsync();

            // Assert
            mockCommand.Verify(c => c.ExecuteScalarAsync(It.IsAny<CancellationToken>()),
                Times.Exactly(Constants.SampleDataCount));

            mockCommand.Verify(c => c.ExecuteNonQueryAsync(It.IsAny<CancellationToken>()),
                Times.Exactly(Constants.SampleDataCount));
        }
    }
}