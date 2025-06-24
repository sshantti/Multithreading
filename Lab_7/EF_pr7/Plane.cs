namespace EF_pr7
{
    public enum EngineType
    {
        Electrical,
        Nuclear,
        Steam
    }
    // Сущность самолета
    public class Plane
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string PlaneCode { get; set; } = string.Empty;
        public EngineType EngineType { get; set; }
        public int ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }
    }
}