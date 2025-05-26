using Xunit;

namespace Concurrency_pr5.Tests
{
    // Тестирование генерации файлов.
    public class GeneratorTests
    {
        // Проверка генерации 5 файлов.
        [Fact]
        public async Task GenerateFilesAsync_CreatesFiveFiles()
        {
            // Arrange: Создание временной директории.
            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            try
            {
                // Act: Генерация файлов.
                await Generator.GenerateFilesAsync(tempDir);

                // Assert: Проверка количества файлов.
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