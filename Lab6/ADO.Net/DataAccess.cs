using System.Data.SqlClient;

namespace ADO.Net
{
    // Предоставляет методы для работы с данными.
    public class DataAccess
    {
        // Добавляет нового производителя в систему.
        public async Task AddManufacturerAsync(string name, string address, bool isChild)
        {
            using var connection = new SqlConnection(Constants.ConnectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(Constants.InsertManufacturer, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@isChild", isChild);
            await command.ExecuteScalarAsync();
        }
        // Добавляет новый самолет в систему
        public async Task AddPlaneAsync(
            string serial,
            string model,
            string code,
            EngineType engine,
            int manufacturerId)
        {
            using var connection = new SqlConnection(Constants.ConnectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(Constants.InsertPlane, connection);
            command.Parameters.AddWithValue("@serial", serial);
            command.Parameters.AddWithValue("@model", model);
            command.Parameters.AddWithValue("@code", code);
            command.Parameters.AddWithValue("@engine", (int)engine);
            command.Parameters.AddWithValue("@manufacturerId", manufacturerId);
            await command.ExecuteNonQueryAsync();
        }

        // Получает все самолеты указанного производителя.
        public async Task<IEnumerable<Plane>> GetPlanesByManufacturerAsync(string manufacturerName)
        {
            var planes = new List<Plane>();

            using var connection = new SqlConnection(Constants.ConnectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(Constants.SelectPlanesByManufacturer, connection);
            command.Parameters.AddWithValue("@name", manufacturerName);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                planes.Add(Plane.Create(
                    serialNumber: reader.GetString(0),
                    model: reader.GetString(1),
                    planeCode: reader.GetString(2),
                    engineType: (EngineType)reader.GetInt32(3)
                ));
            }

            return planes;
        }

        // Получает всех производителей из системы.
        public async Task<IEnumerable<Manufacturer>> GetAllManufacturersAsync()
        {
            var manufacturers = new List<Manufacturer>();

            using var connection = new SqlConnection(Constants.ConnectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(Constants.SelectAllManufacturers, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                manufacturers.Add(new Manufacturer
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Address = reader.GetString(2)
                });
            }

            return manufacturers;
        }
    }
}