using Microsoft.EntityFrameworkCore;

namespace EF_pr7
{
    public class BusinessService
    {
        private readonly AirplaneContext _context;

        // Сервис для бизнес-операций
        public BusinessService(AirplaneContext context)
        {
            _context = context;
        }
        // Добавляет новый самолет и нового производителя
        public async Task AddPlaneWithManufacturerAsync(Plane plane, Manufacturer manufacturer)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Добавление производителя
                _context.Manufacturers.Add(manufacturer);
                await _context.SaveChangesAsync();
                // Привязка самолета к производителю
                plane.ManufacturerId = manufacturer.Id;
                _context.Planes.Add(plane);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        // Получает все самолеты указанного производителя
        public async Task<IEnumerable<Plane>> GetPlanesByManufacturerIdAsync(int manufacturerId)
        {
            return await _context.Planes
                .Where(p => p.ManufacturerId == manufacturerId)
                .ToListAsync();
        }
    }
}