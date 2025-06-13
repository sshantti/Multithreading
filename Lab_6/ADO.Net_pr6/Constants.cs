namespace ADO.Net_pr6
{
    public static class Constants
    {
        // Управление параметрами заполнения БД
        public const int ManufacturerCount = 15;
        public const int PlanesPerManufacturer = 2;
        public const int TotalPlanesCount = ManufacturerCount * PlanesPerManufacturer;

        // Параметры тестирования
        public const int TestManufacturerCount = 5;
        public const int TestPlanesPerManufacturer = 1;
    }
}