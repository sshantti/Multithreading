using EF_pr7;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EF_pr7.Tests
{
    public class BusinessServiceTests
    {
        private readonly DbContextOptions<AirplaneContext> _options;

        public BusinessServiceTests()
        {
            _options = new DbContextOptionsBuilder<AirplaneContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AddPlaneWithManufacturerAsync_ShouldAddBoth()
        {
            using (var context = new AirplaneContext(_options))
            {
                var service = new BusinessService(context);
                var plane = new Plane { SerialNumber = "SN1", Model = "Model1", PlaneCode = "PC1", EngineType = EngineType.Electrical };
                var manufacturer = new Manufacturer { Name = "M1", Address = "A1", IsAChildCompany = false };

                await service.AddPlaneWithManufacturerAsync(plane, manufacturer);
            }

            using (var context = new AirplaneContext(_options))
            {
                Assert.Single(context.Manufacturers);
                Assert.Single(context.Planes);

                var plane = await context.Planes.FirstAsync();
                var manufacturer = await context.Manufacturers.FirstAsync();
                Assert.Equal(manufacturer.Id, plane.ManufacturerId);
            }
        }

        [Fact]
        public async Task AddPlaneWithManufacturerAsync_ShouldRollbackOnFailure()
        {
            using (var context = new AirplaneContext(_options))
            {
                context.Manufacturers.Add(new Manufacturer { Id = 1, Name = "Existing" });
                await context.SaveChangesAsync();
            }

            using (var context = new AirplaneContext(_options))
            {
                var service = new BusinessService(context);
                var plane = new Plane { SerialNumber = "SN1" };
                var manufacturer = new Manufacturer { Id = 1, Name = "M1" };

                await Assert.ThrowsAnyAsync<Exception>(() =>
                    service.AddPlaneWithManufacturerAsync(plane, manufacturer));
            }

            using (var context = new AirplaneContext(_options))
            {
                Assert.Single(context.Manufacturers);
                Assert.Empty(context.Planes);
            }
        }

        [Fact]
        public async Task GetPlanesByManufacturerIdAsync_ShouldReturnCorrectPlanes()
        {
            using (var context = new AirplaneContext(_options))
            {
                var manufacturer1 = new Manufacturer { Name = "M1" };
                var manufacturer2 = new Manufacturer { Name = "M2" };

                context.Manufacturers.AddRange(manufacturer1, manufacturer2);
                await context.SaveChangesAsync();

                context.Planes.AddRange(
                    new Plane { ManufacturerId = manufacturer1.Id, Model = "P1" },
                    new Plane { ManufacturerId = manufacturer1.Id, Model = "P2" },
                    new Plane { ManufacturerId = manufacturer2.Id, Model = "P3" }
                );
                await context.SaveChangesAsync();

                var service = new BusinessService(context);
                var result = await service.GetPlanesByManufacturerIdAsync(manufacturer1.Id);

                Assert.Equal(2, result.Count());
                Assert.All(result, p => Assert.Equal(manufacturer1.Id, p.ManufacturerId));
            }
        }
    }
}