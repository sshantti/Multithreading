using ClassLibrary;

namespace EF_pr7
{
    public class BusinessService
    {
        private readonly AppDbContext _context;
        private readonly IRepository<Manufacturer> _manufacturerRepo;
        private readonly IRepository<Plane> _planeRepo;

        public BusinessService(
            AppDbContext context,
            IRepository<Manufacturer> manufacturerRepo,
            IRepository<Plane> planeRepo)
        {
            _context = context;
            _manufacturerRepo = manufacturerRepo;
            _planeRepo = planeRepo;
        }

        public bool AddNewPlaneForNewManufacturer(
            Manufacturer manufacturer,
            Plane plane)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _manufacturerRepo.Add(manufacturer);
                _manufacturerRepo.Save();

                plane.ManufacturerId = manufacturer.Id;
                _planeRepo.Add(plane);
                _planeRepo.Save();

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }
    }
}