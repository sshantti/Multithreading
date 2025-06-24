namespace EF_pr8
{
    public abstract class Plane
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string PlaneCode { get; set; } = string.Empty;
        public int ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }
    }

    public class ElectricalPlane : Plane
    {
        public double BatteryCapacity { get; set; }
        public int Voltage { get; set; } 
    }

    public class NuclearPlane : Plane
    {
        public string ReactorType { get; set; } = string.Empty;
        public double FuelRodLife { get; set; }
    }

    public class SteamPlane : Plane
    {
        public double BoilerPressure { get; set; }
        public int SteamTemperature { get; set; }
    }
}