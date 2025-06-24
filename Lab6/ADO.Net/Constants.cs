namespace ADO.Net
{
    // Содержит константы для работы с базой данных
    public static class Constants
    {
        public const int SampleDataCount = 30;

        // Настройки подключения
        public const string MasterConnectionString = "Server=(localdb)\\mssqllocaldb;Integrated Security=true;";
        public static string DbName { get; set; } = "AircraftDb";
        public static string ConnectionString => $"Server=(localdb)\\mssqllocaldb;Integrated Security=true;Database={DbName}";

        // Названия таблиц
        public const string ManufacturersTable = "Manufacturers";
        public const string PlanesTable = "Planes";

        // Поля таблицы Manufacturers
        public const string ManufacturerId = "Id";
        public const string ManufacturerName = "Name";
        public const string ManufacturerAddress = "Address";
        public const string ManufacturerIsChildCompany = "IsAChildCompany";

        // Поля таблицы Planes
        public const string PlaneId = "Id";
        public const string PlaneSerialNumber = "SerialNumber";
        public const string PlaneModel = "Model";
        public const string PlaneCode = "PlaneCode";
        public const string PlaneEngineType = "EngineType";
        public const string PlaneManufacturerId = "ManufacturerId";

        // SQL-запросы
        public const string CreateDatabase = "CREATE DATABASE {0}";
        public const string CheckDbExists = "SELECT database_id FROM sys.databases WHERE name = '{0}'";

        // - Названия таблиц и полей
        // - SQL-запросы для создания объектов БД
        public const string CreateManufacturersTable = $@"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{ManufacturersTable}')
        BEGIN
            CREATE TABLE {ManufacturersTable} (
                {ManufacturerId} INT IDENTITY(1,1) PRIMARY KEY,
                {ManufacturerName} NVARCHAR(100) NOT NULL,
                {ManufacturerAddress} NVARCHAR(255) NOT NULL,
                {ManufacturerIsChildCompany} BIT NOT NULL
            )
        END";

        public const string CreatePlanesTable = $@"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{PlanesTable}')
        BEGIN
            CREATE TABLE {PlanesTable} (
                {PlaneId} INT IDENTITY(1,1) PRIMARY KEY,
                {PlaneSerialNumber} NVARCHAR(50) NOT NULL,
                {PlaneModel} NVARCHAR(50) NOT NULL,
                {PlaneCode} NVARCHAR(50) NOT NULL,
                {PlaneEngineType} INT NOT NULL,
                {PlaneManufacturerId} INT NOT NULL,
                FOREIGN KEY ({PlaneManufacturerId}) REFERENCES {ManufacturersTable}({ManufacturerId})
            )
        END";

        public const string InsertManufacturer = $@"
        INSERT INTO {ManufacturersTable} 
        ({ManufacturerName}, {ManufacturerAddress}, {ManufacturerIsChildCompany}) 
        OUTPUT INSERTED.{ManufacturerId}
        VALUES (@name, @address, @isChild)";

        public const string InsertPlane = $@"
        INSERT INTO {PlanesTable} 
        ({PlaneSerialNumber}, {PlaneModel}, {PlaneCode}, {PlaneEngineType}, {PlaneManufacturerId}) 
        VALUES (@serial, @model, @code, @engine, @manufacturerId)";

        public const string SelectPlanesByManufacturer = $@"
        SELECT p.{PlaneSerialNumber}, p.{PlaneModel}, p.{PlaneCode}, p.{PlaneEngineType}
        FROM {PlanesTable} p
        INNER JOIN {ManufacturersTable} m ON p.{PlaneManufacturerId} = m.{ManufacturerId}
        WHERE m.{ManufacturerName} = @name";

        public const string SelectAllManufacturers =
        $"SELECT {ManufacturerId}, {ManufacturerName}, {ManufacturerAddress} " +
        $"FROM {ManufacturersTable}";
    }
}
