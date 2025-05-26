using Xunit;

namespace Concurrency_pr5.Tests
{
    // Тестирование прогресс-бара.
    public class ConsoleProgressTests
    {
        // Проверка обновления прогресса.
        [Fact]
        public void Report_UpdatesProgress()
        {
            // Arrange: Инициализация прогресс-бара
            var progress = new ProgressBar(10);
            // Act: Имитация прогресса
            progress.Report(5);
            progress.Report(5);
        }
    }
}