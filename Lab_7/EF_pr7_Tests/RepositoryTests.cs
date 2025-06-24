using EF_pr7;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EF_pr7.Tests
{
    public class RepositoryTests
    {
        private readonly DbContextOptions<AirplaneContext> _options;

        public RepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AirplaneContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            using (var context = new AirplaneContext(_options))
            {
                var repo = new Repository<Manufacturer>(context);
                var manufacturer = new Manufacturer { Name = "Test", Address = "Test Address" };

                await repo.AddAsync(manufacturer);
                await repo.SaveAsync();

                var result = await context.Manufacturers.FirstOrDefaultAsync();
                Assert.NotNull(result);
                Assert.Equal("Test", result!.Name);
            }
        }

        [Fact]
        public async Task GetAsync_ShouldReturnEntity()
        {
            using (var context = new AirplaneContext(_options))
            {
                var manufacturer = new Manufacturer { Name = "Test" };
                context.Manufacturers.Add(manufacturer);
                await context.SaveChangesAsync();

                var repo = new Repository<Manufacturer>(context);
                var result = await repo.GetAsync(manufacturer.Id);

                Assert.NotNull(result);
                Assert.Equal("Test", result!.Name);
            }
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            using (var context = new AirplaneContext(_options))
            {
                context.Manufacturers.AddRange(
                    new Manufacturer { Name = "Test1" },
                    new Manufacturer { Name = "Test2" }
                );
                await context.SaveChangesAsync();

                var repo = new Repository<Manufacturer>(context);
                var result = await repo.GetAllAsync();

                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public async Task Update_ShouldModifyEntity()
        {
            using (var context = new AirplaneContext(_options))
            {
                var manufacturer = new Manufacturer { Name = "Original" };
                context.Manufacturers.Add(manufacturer);
                await context.SaveChangesAsync();

                manufacturer.Name = "Updated";
                var repo = new Repository<Manufacturer>(context);
                repo.Update(manufacturer);
                await repo.SaveAsync();

                var result = await context.Manufacturers.FindAsync(manufacturer.Id);
                Assert.NotNull(result);
                Assert.Equal("Updated", result!.Name);
            }
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            using (var context = new AirplaneContext(_options))
            {
                var manufacturer = new Manufacturer { Name = "ToDelete" };
                context.Manufacturers.Add(manufacturer);
                await context.SaveChangesAsync();

                var repo = new Repository<Manufacturer>(context);
                repo.Delete(manufacturer);
                await repo.SaveAsync();

                var result = await context.Manufacturers.FindAsync(manufacturer.Id);
                Assert.Null(result);
            }
        }
    }
}