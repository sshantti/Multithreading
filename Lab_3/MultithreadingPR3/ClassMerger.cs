using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using ClassLibrary;

namespace MultithreadingPR3
{
    public class ClassMerger
    {
        private readonly string _filePath1;
        private readonly string _filePath2;
        private readonly string _outputFilePath;
        private readonly object _lockObj = new object();
        private int _currentWriter;

        public ClassMerger(string filePath1, string filePath2, string outputFilePath)
        {
            _filePath1 = filePath1;
            _filePath2 = filePath2;
            _outputFilePath = outputFilePath;
        }

        public void MergeClass()
        {
            File.WriteAllText(_outputFilePath, string.Empty);
            var thread1 = new Thread(() => ProcessFile(_filePath1, 0));
            var thread2 = new Thread(() => ProcessFile(_filePath2, 1));
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
        }
        private void ProcessFile(string filePath, int threadId)
        {
            try
            {
                var data = DeserializeFile(filePath);
                foreach (var item in data)
                {
                    lock (_lockObj)
                    {
                        while (_currentWriter != threadId)
                        {
                            Monitor.Wait(_lockObj);
                        }
                        WriteToFile(item);
                        _currentWriter = (_currentWriter + 1) % 2;
                        Monitor.Pulse(_lockObj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in thread {threadId}: {ex.Message}");
            }
        }
        private List<object> DeserializeFile(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<object>), new[] { typeof(Plane), typeof(Manufacturer) });
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var result = serializer.Deserialize(stream) as List<object>;
            return result ?? new List<object>();
        }
        private void WriteToFile(object item)
        {
            using var writer = new StreamWriter(_outputFilePath, true, Encoding.UTF8);
            writer.WriteLine(ObjectToString(item));
        }
        private string ObjectToString(object obj)
        {
            return obj switch
            {
                Plane plane => string.Format(Constants.PlaneFormat, plane.Model, plane.PlaneCode, plane.EngineType),
                Manufacturer m => string.Format(Constants.ManufacturerFormat, m.Name, m.Address),
                _ => string.Empty
            };
        }
    }
}