using System.Data.SqlClient;

namespace ADO.Net
{
    // Управляет инициализацией базы данных
    public class DatabaseManager
    {
        // Создает БД и таблицы при необходимости
        // 1. Проверяет существование БД
        // 2. Создает БД при отсутствии
        // 3. Создает необходимые таблицы
        public void InitializeDatabase()
        {
            using (var connection = new SqlConnection(Constants.MasterConnectionString))
            {
                connection.Open();
                var cmdCheck = new SqlCommand(
                    string.Format(Constants.CheckDbExists, Constants.DbName),
                    connection);

                var dbId = cmdCheck.ExecuteScalar();
                if (dbId == null)
                {
                    var cmdCreate = new SqlCommand(
                        string.Format(Constants.CreateDatabase, Constants.DbName),
                        connection);
                    cmdCreate.ExecuteNonQuery();
                }
            }

            // Создание таблиц
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                connection.Open();
                ExecuteNonQuery(connection, Constants.CreateManufacturersTable);
                ExecuteNonQuery(connection, Constants.CreatePlanesTable);
            }
        }

        // Вспомогательный метод для выполнения SQL-команд без возврата результатов.
        private void ExecuteNonQuery(SqlConnection connection, string commandText)
        {
            using var command = new SqlCommand(commandText, connection);
            command.ExecuteNonQuery();
        }
    }
}