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

        public void PrintObject()
        {
            Console.WriteLine($"Plane: SerialNumber={SerialNumber}, Model={Model}, PlaneCode={PlaneCode}, EngineType={EngineType}");
        }
    }

    [XmlRoot("Manufacturer")]
    public class Manufacturer
    {
        [XmlElement("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("Address")]
        public string Address { get; set; } = string.Empty;

        [XmlElement("IsAChildCompany")]
        private bool IsAChildCompany { get; set; }

        public static Manufacturer Create(string name, string address, bool isAChildCompany)
        {
            return new Manufacturer
            {
                Name = name,
                Address = address,
                IsAChildCompany = isAChildCompany
            };
        }

        public void PrintObject()
        {
            Console.WriteLine($"Manufacturer: Name={Name}, Address={Address}, IsAChildCompany={IsAChildCompany}");
        }
    }
}