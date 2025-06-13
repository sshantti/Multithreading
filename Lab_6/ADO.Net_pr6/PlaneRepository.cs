using ClassLibrary;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADO.Net_pr6
{
    // Управление данными о самолетах
    // - Добавление новых самолетов
    // - Поиск самолетов по производителю
    // - Поддержка связей с таблицей Manufacturers
    public class PlaneRepository
    {
        private readonly DatabaseContext _context;

        public PlaneRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Plane plane, int manufacturerId)
        {
            const string sql = @"
                INSERT INTO Planes (SerialNumber, Model, PlaneCode, EngineType, ManufacturerId) 
                VALUES (@SerialNumber, @Model, @PlaneCode, @EngineType, @ManufacturerId);
                SELECT SCOPE_IDENTITY();";

            using var command = new SqlCommand(sql, _context.Connection);
            command.Parameters.AddWithValue("@SerialNumber", plane.SerialNumber);
            command.Parameters.AddWithValue("@Model", plane.Model);
            command.Parameters.AddWithValue("@PlaneCode", plane.PlaneCode);
            command.Parameters.AddWithValue("@EngineType", (int)plane.EngineType);
            command.Parameters.AddWithValue("@ManufacturerId", manufacturerId);

            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }

        public async Task<IEnumerable<Plane>> GetByManufacturerIdAsync(int manufacturerId)
        {
            var planes = new List<Plane>();
            const string sql = "SELECT * FROM Planes WHERE ManufacturerId = @ManufacturerId";

            using var command = new SqlCommand(sql, _context.Connection);
            command.Parameters.AddWithValue("@ManufacturerId", manufacturerId);

            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                planes.Add(new Plane
                {
                    Id = (int)reader["Id"],
                    SerialNumber = (string)reader["SerialNumber"],
                    Model = (string)reader["Model"],
                    PlaneCode = (string)reader["PlaneCode"],
                    EngineType = (EngineType)reader["EngineType"]
                });
            }
            return planes;
        }
    }
}