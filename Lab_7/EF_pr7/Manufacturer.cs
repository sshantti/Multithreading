namespace EF_pr7
{
    // Сущность производителя
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsAChildCompany { get; set; }

        // Навигационное свойство для связанных самолетов
        public ICollection<Plane> Planes { get; set; } = new List<Plane>();
    }
}