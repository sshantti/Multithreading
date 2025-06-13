using ADO.Net_pr6;
using ClassLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADO.Net_pr6_Test
{
    [TestClass]
    public class PlaneRepositoryTests
    {
        private const string TestConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=TestDb;Integrated Security=True;";
        private DatabaseContext _context = null!;
        private PlaneRepository _planeRepo = null!;
        private ManufacturerRepository _manRepo = null!;

        [TestInitialize]
        public async Task Initialize()
        {
            _context = new DatabaseContext(TestConnectionString);
            var initializer = new DatabaseInitializer(_context);
            await initializer.InitializeAsync();
            _planeRepo = new PlaneRepository(_context);
            _manRepo = new ManufacturerRepository(_context);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _context.DisposeAsync();
        }

        [TestMethod]
        public async Task AddAsync_ReturnsValidId()
        {
            var manId = await _manRepo.AddAsync(
                Manufacturer.Create("TestMan", "Address", false));

            var plane = Plane.Create("TEST", "MODEL", "CODE", EngineType.Electrical);
            var id = await _planeRepo.AddAsync(plane, manId);
            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public async Task GetByManufacturerIdAsync_ShouldReturnPlanes()
        {
            var manId = await _manRepo.AddAsync(
                Manufacturer.Create("TestMan", "Address", false));

            await _planeRepo.AddAsync(
                Plane.Create("SN1", "Model1", "PC1", EngineType.Electrical),
                manId);

            var planes = await _planeRepo.GetByManufacturerIdAsync(manId);
            Assert.AreEqual(1, planes.Count());
        }
    }
}