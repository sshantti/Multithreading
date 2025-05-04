using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPL
{
    public class Reader
    {
        private readonly string _filePath;
        private readonly TaskFactory _taskFactory;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        public Reader(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            _filePath = filePath;
            _taskFactory = new TaskFactory(
                TaskCreationOptions.LongRunning,
                TaskContinuationOptions.None);
        }
        public async Task ReadFileWithTenThreadsAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var tasks = new Task<string>[10];
            for (int i = 0; i < 10; i++)
            {
                double start = i * 0.1;
                double end = (i + 1) * 0.1;
                tasks[i] = _taskFactory.StartNew(() => ReadFilePart(start, end));
            }

            var results = await Task.WhenAll(tasks);
            Console.WriteLine(string.Concat(results));
            stopwatch.Stop();
            Console.WriteLine(string.Format(Constants.TimeTakenFormat, stopwatch.ElapsedMilliseconds));
        }

        public async Task ReadFileSingleThreadAsync()
        {
            var content = await File.ReadAllTextAsync(_filePath);
            Console.WriteLine(content);
        }

        public async Task ReadFileTwoThreadsAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var tasks = new[]
            {
                _taskFactory.StartNew(() => ReadFilePart(0, 0.5)),
                _taskFactory.StartNew(() => ReadFilePart(0.5, 1.0))
            };

            var results = await Task.WhenAll(tasks);
            Console.WriteLine(string.Concat(results));
            stopwatch.Stop();
            Console.WriteLine(string.Format(Constants.TimeTakenFormat, stopwatch.ElapsedMilliseconds));
        }

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