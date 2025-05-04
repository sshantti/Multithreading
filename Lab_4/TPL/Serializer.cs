using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Library;

namespace TPL
{
    // Выполняет сериализацию объектов в XML-файлы.
    public class Serializer
    {
        private readonly string _filePath1;
        private readonly string _filePath2;
        private readonly List<object> _objects1;
        private readonly List<object> _objects2;

        // Конструктор класса для сериализации данных
        public Serializer(string filePath1, string filePath2, List<object> objects1, List<object> objects2)
        {
            _filePath1 = filePath1 ?? throw new ArgumentNullException(nameof(filePath1));
            _filePath2 = filePath2 ?? throw new ArgumentNullException(nameof(filePath2));
            _objects1 = objects1 ?? throw new ArgumentNullException(nameof(objects1));
            _objects2 = objects2 ?? throw new ArgumentNullException(nameof(objects2));
        }

        // Параллельная запись данных в два файла.
        public async Task SerializeInParallelAsync()
        {
            Task task1 = Task.Run(() => SerializeObjects(_objects1, _filePath1));
            Task task2 = Task.Run(() => SerializeObjects(_objects2, _filePath2));
            await Task.WhenAll(task1, task2);
        }

        // Сериализуются объекты в XML-формат.
        private void SerializeObjects(List<object> objects, string filePath)
        {
            if (objects == null) throw new ArgumentNullException(nameof(objects));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("Invalid file path");

            var serializer = new XmlSerializer(typeof(List<object>), new[] { typeof(Plane), typeof(Manufacturer) });
            using var stream = new FileStream(filePath, FileMode.Create);
            serializer.Serialize(stream, objects);
        }
    }
}