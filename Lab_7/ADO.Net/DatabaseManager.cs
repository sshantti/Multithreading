using System.Data.SqlClient;

namespace ADO.Net
{
    public class DatabaseManager
    {
        public void InitializeDatabase()
        {
            // Проверка существования базы данных
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

        private void ExecuteNonQuery(SqlConnection connection, string commandText)
        {
            using var command = new SqlCommand(commandText, connection);
            command.ExecuteNonQuery();
        }
    }
}
