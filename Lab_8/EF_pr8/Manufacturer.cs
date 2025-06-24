namespace EF_pr8
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsAChildCompany { get; set; }
        public ICollection<Plane> Planes { get; set; } = new List<Plane>();
    }
}