using System;
using System.Xml.Serialization;

namespace ClassLibrary
{
    public enum EngineType
    {
        Electrical,
        Nuclear,
        Steam
    }

    // Класс, представляющий данные
    [XmlRoot("Plane")]
    public class Plane
    {
        [XmlElement("SerialNumber")]
        private string SerialNumber { get; set; } = string.Empty;

        [XmlElement("Model")]
        public string Model { get; set; } = string.Empty;

        [XmlElement("PlaneCode")]
        public string PlaneCode { get; set; } = string.Empty;

        [XmlElement("EngineType")]
        public EngineType EngineType { get; set; }

        // Создает экземпляр.
        public static Plane Create(string serialNumber, string model, string planeCode, EngineType engineType)
        {
            return new Plane
            {
                SerialNumber = serialNumber,
                Model = model,
                PlaneCode = planeCode,
                EngineType = engineType
            };
        }
        // Выводит информацию
        public void PrintObject()
        {
            Console.WriteLine($"Plane: Model={Model}, PlaneCode={PlaneCode}, EngineType={EngineType}");
        }
    }

    // Класс, представляющий данные
    [XmlRoot("Manufacturer")]
    public class Manufacturer
    {
        [XmlElement("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("Address")]
        public string Address { get; set; } = string.Empty;

        [XmlElement("IsAChildCompany")]
        private bool IsAChildCompany { get; set; }

        // Создает экземпляр.
        public static Manufacturer Create(string name, string address, bool isAChildCompany)
        {
            return new Manufacturer
            {
                Name = name,
                Address = address,
                IsAChildCompany = isAChildCompany
            };
        }
        // Выводит информацию.
        public void PrintObject()
        {
            Console.WriteLine($"Manufacturer: Name={Name}, Address={Address}, IsAChildCompany={IsAChildCompany}");
        }
    }
}