namespace TPL
{
    // Содержит константы приложения и строки формата.
    public static class Constants
    {
        public const string File1Path = "file1.xml";
        public const string File2Path = "file2.xml";
        public const string MergedFilePath = "merged.txt";

        public const string PlaneFormat = "Plane: Model={0}, PlaneCode={1}, EngineType={2}";
        public const string ManufacturerFormat = "Manufacturer: Name={0}, Address={1}";

        public const string Task1Completed = "Task 1: Files serialized successfully!";
        public const string Task2Completed = "Task 2: Files merged successfully!";
        public const string Task3SingleThread = "\nTask 3.1: Single-threaded read";
        public const string Task3TwoThreads = "\nTask 3.2: Two-threaded read";
        public const string Task3TenThreads = "\nTask 3.3: Ten threads with semaphore";
        public const string TimeTakenFormat = "Time taken: {0} ms";
        public const int TwoPartsCount = 2;
        public const int TenPartsCount = 10;
        public const int ChildCompanyDivisor = 3;
    }
}