using ADO.Net_pr6;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADO.Net_pr6_Test
{
    [TestClass]
    public class DataSeederTests
    {
        private const string TestConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=TestDb;Integrated Security=True;";

        [TestMethod]
        public async Task SeedAsync_ShouldAdd30Planes()
        {
            await using var context = new DatabaseContext(TestConnectionString);
            var initializer = new DatabaseInitializer(context);
            await initializer.InitializeAsync();

            var seeder = new DataSeeder(
                new ManufacturerRepository(context),
                new PlaneRepository(context));

            await seeder.SeedAsync();

            using var cmd = new SqlCommand(
                "SELECT COUNT(*) FROM Planes",
                context.Connection);

            var result = await cmd.ExecuteScalarAsync();
            var planesCount = result is not null ? Convert.ToInt32(result) : 0;
            Assert.AreEqual(Constants.TotalPlanesCount, planesCount);
        }
    }
}