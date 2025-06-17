using System.Xml.Serialization;
using System;

[XmlRoot("Manufacturer")]
public class Manufacturer
{
    [XmlElement("Id")]
    public int Id { get; set; }

    [XmlElement("Name")]
    public string Name { get; set; } = string.Empty;

    [XmlElement("Address")]
    public string Address { get; set; } = string.Empty;

    [XmlElement("IsAChildCompany")]
    public bool IsAChildCompany { get; set; }

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