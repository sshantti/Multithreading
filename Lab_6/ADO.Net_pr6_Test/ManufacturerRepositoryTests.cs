using ADO.Net_pr6;
using ClassLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADO.Net_pr6_Test
{
    [TestClass]
    public class ManufacturerRepositoryTests
    {
        private const string TestConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=TestDb;Integrated Security=True;";
        private DatabaseContext _context = null!;
        private ManufacturerRepository _repo = null!;

        [TestInitialize]
        public async Task Initialize()
        {
            _context = new DatabaseContext(TestConnectionString);
            var initializer = new DatabaseInitializer(_context);
            await initializer.InitializeAsync();
            _repo = new ManufacturerRepository(_context);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _context.DisposeAsync();
        }

        [TestMethod]
        public async Task AddAsync_ShouldReturnValidId()
        {
            var manufacturer = Manufacturer.Create("TestMan", "TestAddress", true);
            var id = await _repo.AddAsync(manufacturer);
            Assert.IsTrue(id > 0);
        }
    }
}