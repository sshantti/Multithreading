    namespace ClassLibrary
    {
        public enum EngineType 
        { 
            Electrical, 
            Nuclear, 
            Steam 
        }

        public class Plane
        {
            public int Id { get; set; }
            public string SerialNumber { get; set; } = string.Empty;
            public string Model { get; set; } = string.Empty;
            public string PlaneCode { get; set; } = string.Empty;
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
        }

        public class Manufacturer
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public bool IsAChildCompany { get; set; }

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