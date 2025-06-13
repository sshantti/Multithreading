using System;
using Microsoft.Data.SqlClient;

namespace ADO.Net_pr6
{
    // Управление подключением к базе данных
    // - Инкапсуляция соединения с БД
    // - Освобождения ресурсов
    // - Обеспечение безопасного асинхронного закрытия соединения
    public class DatabaseContext : IDisposable, IAsyncDisposable
    {
        private readonly SqlConnection _connection;

        public DatabaseContext(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public SqlConnection Connection => _connection;

        public void Dispose() => _connection?.Dispose();

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
            }
        }
    }
}