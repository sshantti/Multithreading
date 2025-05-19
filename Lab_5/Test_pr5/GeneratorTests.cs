using Xunit;
using System.IO;
using System.Threading.Tasks;
using ClassLibrary;

namespace Concurrency_pr5.Tests
{
    public class GeneratorTests
    {
        [Fact]
        public async Task GenerateFilesAsync_CreatesFiveFiles()
        {
            // Arrange
            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            try
            {
                // Act
                await Generator.GenerateFilesAsync(tempDir);

                // Assert
                var files = Directory.GetFiles(tempDir, "*.xml");
                Assert.Equal(5, files.Length);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}