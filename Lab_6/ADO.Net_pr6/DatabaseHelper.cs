using Microsoft.Data.SqlClient;

namespace ADO.Net_pr6
{
    // Инициализация базы данных
    // - Проверка существования БД
    // - Автоматическое создание БД при отсутствии
    // - Обеспечение готовности инфраструктуры для работы приложения
    public static class DatabaseHelper
    {
        public static void EnsureDatabaseExists(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            var dbName = builder.InitialCatalog;

            builder.InitialCatalog = "master";
            using var masterConn = new SqlConnection(builder.ConnectionString);
            masterConn.Open();

            using var cmd = masterConn.CreateCommand();
            cmd.CommandText = $@"
            IF DB_ID(N'{dbName}') IS NULL
                CREATE DATABASE [{dbName}];";
            cmd.ExecuteNonQuery();
        }
    }
}