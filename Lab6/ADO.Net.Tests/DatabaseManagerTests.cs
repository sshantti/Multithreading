using Moq;
using System.Data.SqlClient;
using Xunit;

namespace ADO.Net.Tests
{
    public class DatabaseManagerTests
    {
        [Fact]
        public void InitializeDatabase_CreatesDatabaseAndTables()
        {
            // Arrange
            var mockMasterConnection = new Mock<SqlConnection>();
            var mockDbConnection = new Mock<SqlConnection>();

            var manager = new DatabaseManager
            {
                CreateConnection = connString =>
                    connString == Constants.MasterConnectionString
                        ? mockMasterConnection.Object
                        : mockDbConnection.Object,
                CreateCommand = (conn, sql) => new SqlCommand(sql, conn),
                ExecuteNonQuery = cmd => { }
            };

            // Act
            manager.InitializeDatabase();

            // Assert
            mockMasterConnection.Verify(c => c.Open(), Times.Once);
            mockDbConnection.Verify(c => c.Open(), Times.Once);
            mockDbConnection.Verify(c => c.Close(), Times.AtLeastOnce);
        }
    }
}