using System;

namespace Concurrency_pr5
{
    // Класс для отображения прогресса в консоли
    public class ProgressBar : IProgress<int>
    {
        private readonly int _total;
        private int _current;
        private readonly object _lock = new();

        public ProgressBar(int total) => _total = total;

        // Обновление прогресса
        public void Report(int value)
        {
            Interlocked.Add(ref _current, value);
            lock (_lock)
            {
                Console.CursorLeft = 0;
                var percent = (int)((double)_current / _total * 100);
                Console.Write($"[{new string('#', percent / 2)}{new string(' ', 50 - percent / 2)}] {percent}%");
            }
        }
    }
}