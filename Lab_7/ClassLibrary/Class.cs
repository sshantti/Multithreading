using System.Collections.Generic;

namespace ClassLibrary
{
    public enum EngineType { Electrical, Nuclear, Steam }

    public class Plane
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string PlaneCode { get; set; } = string.Empty;
        public EngineType EngineType { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }

        public static Plane Create(string serialNumber, string model, string planeCode, EngineType engineType, int manufacturerId)
        {
            return new Plane
            {
                SerialNumber = serialNumber,
                Model = model,
                PlaneCode = planeCode,
                EngineType = engineType,
                ManufacturerId = manufacturerId
            };
        }
    }

    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsAChildCompany { get; set; }

        public ICollection<Plane> Planes { get; set; } = new List<Plane>();

        public static Manufacturer Create(string name, string address, bool isAChildCompany)
        {
            return new Manufacturer
            {
                Name = name,
                Address = address,
                IsAChildCompany = isAChildCompany
            };
        }
    }
}