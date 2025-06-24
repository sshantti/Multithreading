using Xunit;

namespace EF_pr8.Tests
{
    public class ContextFactoryTests
    {
        [Theory]
        [InlineData(InheritanceStrategy.TPH)]
        [InlineData(InheritanceStrategy.TPT)]
        [InlineData(InheritanceStrategy.TPC)]
        public void CreateContext_ShouldReturnContext_WithCorrectStrategy(InheritanceStrategy strategy)
        {
            // Act
            var context = ContextFactory.CreateContext(strategy);

            // Assert
            Assert.NotNull(context);
            Assert.Equal(strategy, context.Strategy);
        }

        [Theory]
        [InlineData(InheritanceStrategy.TPH)]
        [InlineData(InheritanceStrategy.TPT)]
        [InlineData(InheritanceStrategy.TPC)]
        public void CreateInMemoryContext_ShouldReturnContext_WithCorrectStrategy(InheritanceStrategy strategy)
        {
            // Act
            var context = ContextFactory.CreateInMemoryContext(strategy);

            // Assert
            Assert.NotNull(context);
            Assert.Equal(strategy, context.Strategy);
        }
    }
}