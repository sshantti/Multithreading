using Xunit;

namespace EF_pr8.Tests
{
    public class InheritanceStrategyTests
    {
        [Theory]
        [InlineData(InheritanceStrategy.TPH)]
        [InlineData(InheritanceStrategy.TPT)]
        [InlineData(InheritanceStrategy.TPC)]
        public void Strategy_ShouldAffectTableCount(InheritanceStrategy strategy)
        {
            // Arrange
            var context = ContextFactory.CreateInMemoryContext(strategy);

            // Act
            context.Database.EnsureCreated();

            // Assert
            var entityTypes = context.Model.GetEntityTypes().Count();

            switch (strategy)
            {
                case InheritanceStrategy.TPH:
                    Assert.Equal(2, entityTypes); // Planes + Manufacturers
                    break;
                case InheritanceStrategy.TPT:
                    Assert.Equal(5, entityTypes); // Planes + ElectricalPlanes + NuclearPlanes + SteamPlanes + Manufacturers
                    break;
                case InheritanceStrategy.TPC:
                    Assert.Equal(4, entityTypes); // ElectricalPlanes + NuclearPlanes + SteamPlanes + Manufacturers
                    break;
            }
        }
    }
}