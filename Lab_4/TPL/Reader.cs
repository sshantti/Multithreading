using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPL
{
    // Чтение файлов с замером времени.
    public class Reader
    {
        private readonly string _filePath;
        private readonly TaskFactory _taskFactory;
        private readonly CancellationTokenSource _cts = new();

        // Инициализируется экземпляр класса для чтения файлов.
        public Reader(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            _filePath = filePath;
            _taskFactory = new TaskFactory(
                TaskCreationOptions.LongRunning,
                TaskContinuationOptions.None);
        }

        // Чтение файла с использованием 10 потоков.
        //- Каждая часть обрабатывается отдельной задачей.
        // - Использует константу TenPartsCount для определения количества частей.
        public async Task ReadFileWithTenThreadsAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var tasks = new Task<string>[Constants.TenPartsCount];
            double partSize = 1.0 / Constants.TenPartsCount;

            for (int i = 0; i < Constants.TenPartsCount; i++)
            {
                double start = i * partSize;
                double end = (i + 1) * partSize;
                tasks[i] = _taskFactory.StartNew(() => ReadFilePart(start, end));
            }

            var results = await Task.WhenAll(tasks);
            Console.WriteLine(string.Concat(results));
            stopwatch.Stop();
            Console.WriteLine(string.Format(Constants.TimeTakenFormat, stopwatch.ElapsedMilliseconds));
        }

        // Читает весь файл в одном потоке.
        // - Использует File.ReadAllTextAsync для асинхронного чтения.
        public async Task ReadFileSingleThreadAsync()
        {
            var content = await File.ReadAllTextAsync(_filePath);
            Console.WriteLine(content);
        }

        // Читает файл, разделяя его на две части.
        // - Первая половина и вторая половина файла обрабатываются параллельно.
        public async Task ReadFileTwoThreadsAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var tasks = new Task<string>[Constants.TwoPartsCount];
            double partSize = 1.0 / Constants.TwoPartsCount;

            for (int i = 0; i < Constants.TwoPartsCount; i++)
            {
                double start = i * partSize;
                double end = (i + 1) * partSize;
                tasks[i] = _taskFactory.StartNew(() => ReadFilePart(start, end));
            }

            var results = await Task.WhenAll(tasks);
            Console.WriteLine(string.Concat(results));
            stopwatch.Stop();
            Console.WriteLine(string.Format(Constants.TimeTakenFormat, stopwatch.ElapsedMilliseconds));
        }

        // Читает часть файла в заданном диапазоне (от startRatio до endRatio).
        // - FileOptions.SequentialScan: Указывает ОС, что файл читается последовательно.
        // - FileShare.ReadWrite: Позволяет другим процессам читать и изменять файл.
        private string ReadFilePart(double startRatio, double endRatio)
        {
            using var stream = new FileStream(
                _filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite,
                bufferSize: 4096,
                FileOptions.SequentialScan
            );
            using var reader = new StreamReader(stream, Encoding.UTF8);

            var start = (long)(stream.Length * startRatio);
            stream.Seek(start, SeekOrigin.Begin);

            var buffer = new char[(int)(stream.Length * (endRatio - startRatio))];
            reader.ReadBlock(buffer, 0, buffer.Length);

            return new string(buffer);
        }

        public void CancelAllOperations() => _cts.Cancel();
    }
}