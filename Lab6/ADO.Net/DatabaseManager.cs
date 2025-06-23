using System.Data.SqlClient;

namespace ADO.Net
{
    // Управляет инициализацией базы данных
    public class DatabaseManager
    {
        public Func<string, SqlConnection> CreateConnection { get; set; } = 
            connectionString => new SqlConnection(connectionString);
        
        public Func<SqlConnection, string, SqlCommand> CreateCommand { get; set; } = 
            (conn, sql) => new SqlCommand(sql, conn);
        
        public Action<SqlCommand> ExecuteNonQuery { get; set; } = 
            cmd => cmd.ExecuteNonQuery();

        // Создает БД и таблицы при необходимости
        // 1. Проверяет существование БД
        // 2. Создает БД при отсутствии
        // 3. Создает необходимые таблицы
        public void InitializeDatabase()
        {
            using (var connection = CreateConnection(Constants.MasterConnectionString))
            {
                connection.Open();
                var cmdCheck = CreateCommand(connection, string.Format(Constants.CheckDbExists, Constants.DbName));
                
                var dbId = cmdCheck.ExecuteScalar();
                if (dbId == null)
                {
                    var cmdCreate = CreateCommand(connection, string.Format(Constants.CreateDatabase, Constants.DbName));
                    ExecuteNonQuery(cmdCreate);
                }
            }
            // Создание таблиц
            using (var connection = CreateConnection(Constants.ConnectionString))
            {
                connection.Open();
                ExecuteTableCreation(connection, Constants.CreateManufacturersTable);
                ExecuteTableCreation(connection, Constants.CreatePlanesTable);
            }
        }

        // Вспомогательный метод для выполнения SQL-команд.
        private void ExecuteTableCreation(SqlConnection connection, string commandText)
        {
            var command = CreateCommand(connection, commandText);
            ExecuteNonQuery(command);
        }
    }
}