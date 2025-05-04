using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Library;

namespace TPL
{
    // Обработка файловых операций
    public class FileProcessor
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly string _file1;
        private readonly string _file2;
        private readonly string _mergedFile;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly TaskFactory _taskFactory;

        // Инициализирует файловый процессор с настройками задач.
        public FileProcessor(string file1, string file2, string mergedFile)
        {
            _file1 = file1;
            _file2 = file2;
            _mergedFile = mergedFile;

            _taskFactory = new TaskFactory(TaskCreationOptions.AttachedToParent,
                TaskContinuationOptions.DenyChildAttach);
        }

        // Задание 1. Параллельно генерируется тестовые данные и записываются в файлы.
        public async Task GenerateAndWriteFilesAsync()
        {
            var objects1 = GenerateObjects(0, 10);
            var objects2 = GenerateObjects(10, 20);

            var task1 = _taskFactory.StartNew(
                () => SerializeObjectsAsync(objects1, _file1, _cts.Token),
                _cts.Token);

            var task2 = _taskFactory.StartNew(
                () => SerializeObjectsAsync(objects2, _file2, _cts.Token),
                _cts.Token);

            await Task.WhenAll(task1, task2).ContinueWith(_ =>
                _cts.Token.ThrowIfCancellationRequested());
        }

        // Задание 2. Выполняется слияние данных из 2 файлов в 1.

        public async Task MergeFilesAsync()
        {
            await using var writer = new StreamWriter(_mergedFile);

            var data1 = await ReadFileAsync(_file1, _cts.Token);
            var data2 = await ReadFileAsync(_file2, _cts.Token);

            foreach (var item in data1)
            {
                await WriteToFileAsync(writer, item, _cts.Token);
                await writer.FlushAsync();
            }

            foreach (var item in data2)
            {
                await WriteToFileAsync(writer, item, _cts.Token);
                await writer.FlushAsync();
            }
        }

        // Задание 3.
        public async Task ReadAndPrintMergedFileAsync()
        {
            var data = await ReadFileAsync(_mergedFile, _cts.Token);
            var outputTasks = new List<Task>();

            for (int i = 0; i < data.Count; i++)
            {
                var index = i;
                var item = data[index];
                outputTasks.Add(_taskFactory.StartNew(
                    () => PrintObjectAsync(item, index, _cts.Token),
                    _cts.Token));
            }

            await Task.WhenAll(outputTasks);
            Console.WriteLine("All print tasks completed");
        }

        // Создается список тестовых объектов.
        private List<object> GenerateObjects(int start, int end)
        {
            var list = new List<object>();
            for (int i = start; i < end; i++)
            {
                if (i % 2 == 0)
                    list.Add(Plane.Create($"SN{i}", $"Model{i}", $"PC{i}", EngineType.Electrical));
                else
                    list.Add(Manufacturer.Create($"Name{i}", $"Address{i}", i % 3 == 0));
            }
            return list;
        }

        // Асинхронно записываются сериализованные данные в файл.
        private async Task SerializeObjectsAsync(List<object> objects, string path, CancellationToken token)
        {
            await using var stream = new FileStream(path, FileMode.Create);
            var serializer = new XmlSerializer(typeof(List<object>), new[] { typeof(Plane), typeof(Manufacturer) });
            var xmlString = SerializeObject(serializer, objects);
            var bytes = Encoding.UTF8.GetBytes(xmlString);
            await stream.WriteAsync(bytes, 0, bytes.Length, token);
        }

        // Преобразует список объектов в XML-строку.
        private string SerializeObject(XmlSerializer serializer, List<object> objects)
        {
            using var writer = new StringWriter();
            serializer.Serialize(writer, objects);
            return writer.ToString();
        }

        // Десериализует данные из XML-файла.
        private async Task<List<object>> ReadFileAsync(string path, CancellationToken token)
        {
            await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            var serializer = new XmlSerializer(typeof(List<object>), new[] { typeof(Plane), typeof(Manufacturer) });

            return await _taskFactory.StartNew(() =>
            {
                token.ThrowIfCancellationRequested();
                var result = serializer.Deserialize(stream) as List<object>;
                return result ?? throw new InvalidOperationException("Deserialization failed");
            }, token);
        }

        // Асинхронно записывается объект в файл с синхронизацией доступа.
        private async Task WriteToFileAsync(StreamWriter writer, object item, CancellationToken token)
        {
            await _semaphore.WaitAsync(token);
            try
            {
                await writer.WriteLineAsync(ObjectToString(item).AsMemory(), token);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        // Выводит информацию об объекте в консоль.
        private async Task PrintObjectAsync(object obj, int index, CancellationToken token)
        {
            await Console.Out.WriteLineAsync($"Item {index}: {ObjectToString(obj)}");
        }

        // Преобразуется объект в строковое представление.
        private string ObjectToString(object obj) => obj switch
        {
            Plane p => string.Format(Constants.PlaneFormat, p.Model, p.PlaneCode, p.EngineType),
            Manufacturer m => string.Format(Constants.ManufacturerFormat, m.Name, m.Address),
            _ => string.Empty
        };

        public void CancelAllOperations() => _cts.Cancel();
    }
}