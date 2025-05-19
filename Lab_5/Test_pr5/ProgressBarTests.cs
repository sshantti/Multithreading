using Xunit;
using System;

namespace Concurrency_pr5.Tests
{
    public class ConsoleProgressTests
    {
        [Fact]
        public void Report_UpdatesProgress()
        {
            // Arrange
            var progress = new ProgressBar(10);
            // Act & Assert (проверяем отсутствие исключений)
            progress.Report(5);
            progress.Report(5);
        }
    }
}