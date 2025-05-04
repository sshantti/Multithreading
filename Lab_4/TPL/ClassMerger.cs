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
    public class ClassMerger
    {
        private readonly string _filePath1;
        private readonly string _filePath2;
        private readonly string _outputFilePath;
        private readonly TaskFactory _taskFactory;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public ClassMerger(string filePath1, string filePath2, string outputFilePath)
        {
            _filePath1 = filePath1;
            _filePath2 = filePath2;
            _outputFilePath = outputFilePath;

            _taskFactory = new TaskFactory(_cts.Token);
        }

        public async Task MergeFilesAsync()
        {
            try
            {
                var data1 = await ReadFileAsync(_filePath1);
                var data2 = await ReadFileAsync(_filePath2);

                await using var writer = new StreamWriter(_outputFilePath);

                foreach (var item in data1)
                    await writer.WriteLineAsync(ObjectToString(item));

                foreach (var item in data2)
                    await writer.WriteLineAsync(ObjectToString(item));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when merging files: {ex.Message}");
                throw;
            }
        }

        private async Task<List<object>> ReadFileAsync(string filePath)
        {
            try
            {
                await using var stream = new FileStream(filePath, FileMode.Open);
                var serializer = new XmlSerializer(typeof(List<object>), new[] { typeof(Plane), typeof(Manufacturer) });
                return (List<object>)serializer.Deserialize(stream)!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File reading error {filePath}: {ex.Message}");
                throw;
            }
        }

        private static string ObjectToString(object obj) => obj switch
        {
            Plane p => string.Format(Constants.PlaneFormat, p.Model, p.PlaneCode, p.EngineType),
            Manufacturer m => string.Format(Constants.ManufacturerFormat, m.Name, m.Address),
            _ => string.Empty
        };

        public void CancelOperation() => _cts.Cancel();
    }
}