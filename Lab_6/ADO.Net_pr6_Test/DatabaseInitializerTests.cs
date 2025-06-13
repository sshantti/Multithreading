using ADO.Net_pr6;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADO.Net_pr6_Test
{
    [TestClass]
    public class DatabaseInitializerTests
    {
        private const string TestConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=TestDb;Integrated Security=True;";

        [TestMethod]
        public async Task InitializeAsync_CreatesTables()
        {
            await using var context = new DatabaseContext(TestConnectionString);
            var initializer = new DatabaseInitializer(context);
            await initializer.InitializeAsync();

            using var cmd = new SqlCommand(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES " +
                "WHERE TABLE_NAME IN ('Manufacturers', 'Planes')",
                context.Connection);

            var count = (int)await cmd.ExecuteScalarAsync();
            Assert.AreEqual(2, count);
        }
    }
}