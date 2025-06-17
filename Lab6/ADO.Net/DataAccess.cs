using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ADO.Net
{
    public class DataAccess
    {
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
