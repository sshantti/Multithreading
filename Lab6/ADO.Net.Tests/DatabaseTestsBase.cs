using System.Data.SqlClient;
using Xunit;

namespace ADO.Net.Tests
{
    public class DatabaseTestsBase : IAsyncLifetime
    {
        protected const string TestDbName = "ADO.Net.Tests";

        public async Task InitializeAsync()
        {
            await CreateTestDatabaseAsync();
            Constants.DbName = TestDbName;

            var dbManager = new DatabaseManager();
            dbManager.InitializeDatabase();

            await ClearDatabaseAsync();
        }

        public async Task DisposeAsync()
        {
            await DropTestDatabaseAsync();
        }

        private async Task CreateTestDatabaseAsync()
        {
            using var connection = new SqlConnection(Constants.MasterConnectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand($"CREATE DATABASE {TestDbName}", connection);
            await command.ExecuteNonQueryAsync();
        }

        private async Task DropTestDatabaseAsync()
        {
            using var connection = new SqlConnection(Constants.MasterConnectionString);
            await connection.OpenAsync();

            try
            {
                var killConnections = $@"
                    DECLARE @kill varchar(8000) = '';  
                    SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'  
                    FROM sys.dm_exec_sessions
                    WHERE database_id = DB_ID('{TestDbName}');
                    EXEC(@kill);";

                using var killCommand = new SqlCommand(killConnections, connection);
                await killCommand.ExecuteNonQueryAsync();

                using var dropCommand = new SqlCommand($"DROP DATABASE IF EXISTS {TestDbName}", connection);
                await dropCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error dropping test database: {ex.Message}");
            }
        }

        private async Task ClearDatabaseAsync()
        {
            using var connection = new SqlConnection(Constants.ConnectionString);
            await connection.OpenAsync();

            var commands = new[]
            {
                $"DELETE FROM {Constants.PlanesTable}",
                $"DELETE FROM {Constants.ManufacturersTable}",
                $"DBCC CHECKIDENT ('{Constants.ManufacturersTable}', RESEED, 0)",
                $"DBCC CHECKIDENT ('{Constants.PlanesTable}', RESEED, 0)"
            };

            foreach (var cmdText in commands)
            {
                using var command = new SqlCommand(cmdText, connection);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}