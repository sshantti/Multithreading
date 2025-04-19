using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ClassLibrary;

namespace MultithreadingPR3
{
    public class Serializer
    {
        private readonly string _filePath1;
        private readonly string _filePath2;
        private readonly List<object> _objects1;
        private readonly List<object> _objects2;

        public Serializer(string filePath1, string filePath2, List<object> objects1, List<object> objects2)
        {
            _filePath1 = filePath1 ?? throw new ArgumentNullException(nameof(filePath1));
            _filePath2 = filePath2 ?? throw new ArgumentNullException(nameof(filePath2));
            _objects1 = objects1 ?? throw new ArgumentNullException(nameof(objects1));
            _objects2 = objects2 ?? throw new ArgumentNullException(nameof(objects2));
        }

        public void SerializeInParallel()
        {
            Thread thread1 = new Thread(() => SerializeObjects(_objects1, _filePath1));
            Thread thread2 = new Thread(() => SerializeObjects(_objects2, _filePath2));

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
        }

        private void SerializeObjects(List<object> objects, string filePath)
        {
            if (objects == null) throw new ArgumentNullException(nameof(objects));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be empty", nameof(filePath));

            var serializer = new XmlSerializer(typeof(List<object>), new[] { typeof(Plane), typeof(Manufacturer) });
            using var stream = new FileStream(filePath, FileMode.Create);
            serializer.Serialize(stream, objects);
        }
    }
}