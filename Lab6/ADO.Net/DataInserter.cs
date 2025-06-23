using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ADO.Net
{
    // Заполняет базу данных тестовыми данными.
    public class DataInserter
    {
        public Func<SqlConnection> CreateConnection { get; set; } =
            () => new SqlConnection(Constants.ConnectionString);

        public Func<SqlConnection, string, SqlCommand> CreateCommand { get; set; } =
            (conn, sql) => new SqlCommand(sql, conn);

        // Асинхронно добавляет 30 производителей и 30 самолетов
        public async Task InsertDataAsync()
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            for (int i = 1; i <= Constants.SampleDataCount; i++)
            {
                var manufacturerId = await InsertManufacturerAsync(
                    connection,
                    $"Manufacturer_{i}",
                    $"Address_{i}",
                    i % 2 == 0
                );

                await InsertPlaneAsync(
                    connection,
                    $"SN_{i}",
                    $"Model_{i}",
                    $"Code_{i}",
                    (EngineType)(i % 3),
                    manufacturerId
                );
            }
        }
        // Добавляет производителя в базу данных
        private async Task<int> InsertManufacturerAsync(SqlConnection connection, string name, string address, bool isChild)
        {
            using var command = CreateCommand(connection, Constants.InsertManufacturer);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@isChild", isChild);

            var result = await command.ExecuteScalarAsync();
            return result != null ? (int)result : 0;
        }
        // Добавляет самолет в базу данных
        private async Task InsertPlaneAsync(SqlConnection connection, string serial, string model, string code, EngineType engine, int manufacturerId)
        {
            using var command = CreateCommand(connection, Constants.InsertPlane);
            command.Parameters.AddWithValue("@serial", serial);
            command.Parameters.AddWithValue("@model", model);
            command.Parameters.AddWithValue("@code", code);
            command.Parameters.AddWithValue("@engine", (int)engine);
            command.Parameters.AddWithValue("@manufacturerId", manufacturerId);
            await command.ExecuteNonQueryAsync();
        }
    }
}