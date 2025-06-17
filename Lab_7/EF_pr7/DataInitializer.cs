using Microsoft.EntityFrameworkCore;
using ClassLibrary;
using System.Collections.Generic;
using System.Linq;

namespace EF_pr7
{
    public static class DataInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();

            if (context.Manufacturers.Any()) return;

            var random = new Random();
            var manufacturers = new List<Manufacturer>();

            for (int i = 1; i <= 30; i++)
            {
                manufacturers.Add(new Manufacturer
                {
                    Name = $"Manufacturer_{i}",
                    Address = $"Address_{i}",
                    IsAChildCompany = (i % 4 == 0)
                });
            }
            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            var planes = new List<Plane>();
            for (int i = 1; i <= 30; i++)
            {
                planes.Add(new Plane
                {
                    SerialNumber = $"SN-{i}",
                    Model = $"Model_{i % 10}",
                    PlaneCode = $"PC-{i}",
                    EngineType = (EngineType)(i % 3),
                    ManufacturerId = manufacturers[random.Next(manufacturers.Count)].Id
                });
            }
            context.Planes.AddRange(planes);
            context.SaveChanges();
        }
    }
}