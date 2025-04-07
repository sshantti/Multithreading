using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SerializationLibrary;

namespace SerializationPR2.Services
{
    public sealed class ObjectService
    {
        public const string DefaultXmlFileName = "objects.xml";
        private readonly List<object> _objects = new();

        public void CreateAndDisplayObjects()
        {
            _objects.Clear();
            var random = new Random();

            for (var i = 0; i < 5; i++)
            {
                var plane = Plane.Create(
                    $"SN{i + 1}",
                    $"Model{random.Next(100, 999)}",
                    $"PC{random.Next(1000, 9999)}",
                    (EngineType)random.Next(0, 3)
                    );
                _objects.Add(plane);
                plane.PrintObject();
            }

            for (var i = 0; i < 5; i++)
            {
                var manufacturer = Manufacturer.Create(
                    $"Manufacturer{i + 1}",
                    $"Address{i + 1}",
                    random.Next(2) == 0
                );
                _objects.Add(manufacturer);
                manufacturer.PrintObject();
            }
        }
        public void SerializeToXml()
        {
            string filePath = DefaultXmlFileName;
            var serializer = new XmlSerializer(typeof(List<object>), new[]
            {
            typeof(Plane),
            typeof(Manufacturer)
        });

            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, _objects);
        }
        public static void ReadAndDisplayObjects()
        {
            string filePath = DefaultXmlFileName;

            if (!File.Exists(filePath))
            {
                Console.WriteLine("XML file does not exist");
                return;
            }
            var serializer = new XmlSerializer(typeof(List<object>), new[]
        {
            typeof(Plane),
            typeof(Manufacturer)
        });

            using var reader = new StreamReader(filePath);
            if (serializer.Deserialize(reader) is List<object> deserializedObjects)
            {
                foreach (var obj in deserializedObjects)
                {
                    if (obj is Plane plane)
                        plane.PrintObject();
                    else if (obj is Manufacturer manufacturer)
                        manufacturer.PrintObject();
                }
            }
        }
    }
}