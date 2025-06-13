using Microsoft.Data.SqlClient;

namespace ADO.Net_pr6
{
    // Инициализация структуры базы данных
    // - Создание таблиц Manufacturers и Planes
    // - Определение связей между таблицами
    // - Гарантия согласованности структуры БД
    public class DatabaseInitializer
    {
        private readonly DatabaseContext _context;

        public DatabaseInitializer(DatabaseContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            const string createManufacturers = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Manufacturers')
            CREATE TABLE Manufacturers (
                ID INT IDENTITY(1,1) PRIMARY KEY,
                Name NVARCHAR(100) NOT NULL,
                Address NVARCHAR(255),
                IsAChildCompany BIT NOT NULL
            )";

            const string createPlanes = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Planes')
            CREATE TABLE Planes (
                ID INT IDENTITY(1,1) PRIMARY KEY,
                SerialNumber NVARCHAR(50) NOT NULL,
                Model NVARCHAR(50) NOT NULL,
                PlaneCode NVARCHAR(20) NOT NULL,
                EngineType INT NOT NULL,
                ManufacturerID INT NOT NULL,
                FOREIGN KEY (ManufacturerId) REFERENCES Manufacturers(Id)
            )";

            await ExecuteCommandAsync(createManufacturers);
            await ExecuteCommandAsync(createPlanes);
        }

        private async Task ExecuteCommandAsync(string sql)
        {
            using var command = new SqlCommand(sql, _context.Connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
