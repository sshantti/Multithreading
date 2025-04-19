using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace MultithreadingPR3
{
    public class Reader
    {
        private readonly string _filePath;
        private readonly Mutex _mutex = new Mutex();

        public Reader(string filePath)
        {
            _filePath = filePath;
        }

        public void ReadFileSingleThread()
        {
            var stopwatch = Stopwatch.StartNew();
            _mutex.WaitOne();
            try
            {
                string content = File.ReadAllText(_filePath);
                Console.WriteLine(content);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
            stopwatch.Stop();
            Console.WriteLine(string.Format(Constants.TimeTakenFormat, stopwatch.ElapsedMilliseconds));
        }

        public void ReadFileTwoThreads()
        {
            var stopwatch = Stopwatch.StartNew();
            var results = new string[2];
            var events = new ManualResetEvent[2];

            for (int i = 0; i < 2; i++)
            {
                events[i] = new ManualResetEvent(false);
                int index = i;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    results[index] = ReadFilePart(index * 0.5, (index + 1) * 0.5);
                    events[index].Set();
                });
            }

            WaitHandle.WaitAll(events);
            Console.WriteLine(results[0] + results[1]);
            stopwatch.Stop();
            Console.WriteLine(string.Format(Constants.TimeTakenFormat, stopwatch.ElapsedMilliseconds));
        }
        public void ReadFileWithTenThreads()
        {
            var stopwatch = Stopwatch.StartNew();
            var results = new string[10];
            var events = new ManualResetEvent[10];
            var semaphore = new Semaphore(5, 5);

            for (int i = 0; i < 10; i++)
            {
                events[i] = new ManualResetEvent(false);
                int index = i;
                new Thread(() =>
                {
                    semaphore.WaitOne();
                    try
                    {
                        results[index] = ReadFilePart(index * 0.1, (index + 1) * 0.1);
                    }
                    finally
                    {
                        semaphore.Release();
                        events[index].Set();
                    }
                }).Start();
            }

            WaitHandle.WaitAll(events);
            Console.WriteLine(string.Concat(results));
            stopwatch.Stop();
            Console.WriteLine(string.Format(Constants.TimeTakenFormat, stopwatch.ElapsedMilliseconds));
        }
        private string ReadFilePart(double startRatio, double endRatio)
        {
            _mutex.WaitOne();
            try
            {
                using var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                long start = (long)(stream.Length * startRatio);
                long end = (long)(stream.Length * endRatio);
                long length = end - start;
                stream.Seek(start, SeekOrigin.Begin);
                byte[] buffer = new byte[length];
                stream.Read(buffer, 0, (int)length);
                return Encoding.UTF8.GetString(buffer);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
    }
}